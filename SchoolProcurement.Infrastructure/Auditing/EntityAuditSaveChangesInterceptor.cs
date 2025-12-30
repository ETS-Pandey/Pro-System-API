using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure.Security;

namespace SchoolProcurement.Infrastructure.Auditing
{
    public class EntityAuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        // No scoped services injected in ctor
        public EntityAuditSaveChangesInterceptor() { }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null)
                return await base.SavingChangesAsync(eventData, result, cancellationToken);

            // --- 1) Resolve IHttpContextAccessor (singleton) safely from the DbContext's internal provider ---
            // IHttpContextAccessor is registered as a singleton via builder.Services.AddHttpContextAccessor()
            var httpContextAccessor = context.GetService<IHttpContextAccessor>();

            int? userId = null;
            string? userName = null;
            Guid corrId = Guid.Empty;

            if (httpContextAccessor?.HttpContext != null)
            {
                var httpContext = httpContextAccessor.HttpContext;

                // Try read correlation id from header (if client provided it)
                var corrHeader = httpContext.Request.Headers["X-Correlation-Id"].FirstOrDefault();
                if (Guid.TryParse(corrHeader, out var parsedCorr))
                    corrId = parsedCorr;

                // Try to get claims-based user id / name from HttpContext.User
                var user = httpContext.User;
                if (user?.Identity != null && user.Identity.IsAuthenticated)
                {
                    // Try common claim types for id and name
                    var subClaim = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                                   ?? user.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

                    if (subClaim != null && int.TryParse(subClaim.Value, out var parsedId))
                        userId = parsedId;

                    var nameClaim = user.FindFirst(System.Security.Claims.ClaimTypes.Name)
                                    ?? user.FindFirst("name")
                                    ?? user.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Name);

                    if (nameClaim != null)
                        userName = nameClaim.Value;
                }
            }
            else
            {
                // No HttpContext available (background job or CLI). Don't attempt to resolve scoped ICurrentUserService from root.
                // Optionally, you could try to get a system user id from the DbContext or environment here.
            }

            // Fallback: if you *must* check the scoped CurrentUserService, do so *only* from the context's scoped provider
            // and only if it is safe — but avoid doing that to prevent root-provider resolution issues.
            // Example (commented out): 
            // var currentUser = context.GetService<ICurrentUserService>();
            // if (currentUser != null) { userId ??= currentUser.UserId; userName ??= currentUser.UserName; corrId = corrId == Guid.Empty ? currentUser.CorrelationId : corrId; }

            // Collect tracked entity entries (ignore AuditLog/AuditLogDetail)
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .Where(e => e.Entity is not AuditLog && e.Entity is not AuditLogDetail)
                .ToList();

            if (!entries.Any())
                return await base.SavingChangesAsync(eventData, result, cancellationToken);

            var audit = new AuditLog
            {
                CorrelationId = corrId == Guid.Empty ? Guid.NewGuid() : corrId,
                EventTime = DateTime.UtcNow,
                EventType = "EntityChange",
                UserId = userId,
                UserName = userName,
                Source = "EFCore",
                Summary = $"Entity change: {entries.Count} entries"
            };

            foreach (var entry in entries)
            {
                var entityName = entry.Entity.GetType().Name;
                var pk = GetPrimaryKeyValue(entry);
                var op = entry.State == EntityState.Added ? "Insert" : entry.State == EntityState.Deleted ? "Delete" : "Update";

                if (entry.State == EntityState.Added)
                {
                    foreach (var prop in entry.Properties)
                    {
                        if (IsSensitive(prop.Metadata.Name)) continue;
                        var newVal = prop.CurrentValue != null ? JsonConvert.SerializeObject(prop.CurrentValue) : null;
                        audit.Details.Add(new AuditLogDetail
                        {
                            EntityName = entityName,
                            PrimaryKey = pk,
                            Operation = op,
                            PropertyName = prop.Metadata.Name,
                            OldValue = null,
                            NewValue = newVal
                        });
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    foreach (var prop in entry.Properties)
                    {
                        if (IsSensitive(prop.Metadata.Name)) continue;
                        var oldVal = prop.OriginalValue != null ? JsonConvert.SerializeObject(prop.OriginalValue) : null;
                        audit.Details.Add(new AuditLogDetail
                        {
                            EntityName = entityName,
                            PrimaryKey = pk,
                            Operation = op,
                            PropertyName = prop.Metadata.Name,
                            OldValue = oldVal,
                            NewValue = null
                        });
                    }
                }
                else // Modified
                {
                    foreach (var prop in entry.Properties.Where(p => p.IsModified))
                    {
                        if (IsSensitive(prop.Metadata.Name)) continue;
                        var oldVal = prop.OriginalValue != null ? JsonConvert.SerializeObject(prop.OriginalValue) : null;
                        var newVal = prop.CurrentValue != null ? JsonConvert.SerializeObject(prop.CurrentValue) : null;
                        audit.Details.Add(new AuditLogDetail
                        {
                            EntityName = entityName,
                            PrimaryKey = pk,
                            Operation = op,
                            PropertyName = prop.Metadata.Name,
                            OldValue = oldVal,
                            NewValue = newVal
                        });
                    }
                }
            }

            // Add the audit header (and details) to the same DbContext so they commit/rollback together
            context.Set<AuditLog>().Add(audit);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static bool IsSensitive(string propertyName)
        {
            var sensitive = new[] { "Password", "Saltkey", "UniqueKey", "Token" };
            return sensitive.Contains(propertyName, StringComparer.OrdinalIgnoreCase);
        }

        private static string? GetPrimaryKeyValue(EntityEntry entry)
        {
            var key = entry.Metadata.FindPrimaryKey();
            if (key == null) return null;
            var values = key.Properties.Select(p =>
            {
                var cur = entry.Property(p.Name).CurrentValue;
                var orig = entry.Property(p.Name).OriginalValue;
                return (cur ?? orig)?.ToString();
            }).Where(s => s != null).Select(s => s!).ToArray();
            return string.Join(";", values);
        }
    }
}
