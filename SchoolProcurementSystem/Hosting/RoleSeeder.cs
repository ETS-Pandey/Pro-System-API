using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure.Persistence;

namespace SchoolProcurement.Api.Hosting
{
    public static class RoleSeeder
    {
        private static readonly string[] DefaultRoles = new[]
        {
            "SuperAdmin",
            "BranchAdmin",
            "AdminHRAdmin",
            "Procurement",
            "Department",
            "Principal",
            "Teacher",
            "Clinic",
            "Staff"
        };

        /// <summary>
        /// Seed default roles. This method intentionally creates its own DbContext instance
        /// (bypassing the app's DbContext registrations) to avoid resolving scoped startup services.
        /// </summary>
        public static async Task SeedAsync(IServiceProvider services, ILogger? logger = null, CancellationToken ct = default)
        {
            // Get configuration so we can build a fresh DbContextOptions (no interceptors, no app DI)
            var config = services.GetService<IConfiguration>()
                         ?? throw new InvalidOperationException("IConfiguration is required for RoleSeeder.");

            // prefer connection from appsettings, then env var
            var conn = config.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("No connection string found for RoleSeeder.");

            var optionsBuilder = new DbContextOptionsBuilder<SchoolDbContext>();
            optionsBuilder.UseSqlServer(conn);

            // IMPORTANT: do NOT add interceptors here and do NOT use the app-level service provider's DbContext.
            // This ensures the seeder doesn't trigger startup-time scoped service resolution.
            await using var db = new SchoolDbContext(optionsBuilder.Options);

            try
            {
                // Fast check whether DB exists / can connect
                if (!await db.Database.CanConnectAsync(ct))
                {
                    logger?.LogWarning("RoleSeeder: database cannot connect. Skipping seeding.");
                    return;
                }

                // Query existing role names
                var existing = await db.Roles.AsNoTracking()
                    .Select(r => r.Name)
                    .ToListAsync(ct);

                var existingSet = new HashSet<string>(existing, StringComparer.OrdinalIgnoreCase);

                var toInsert = DefaultRoles
                    .Where(name => !existingSet.Contains(name))
                    .Select(name => new Role { Name = name, CreatedDate = DateTime.UtcNow })
                    .ToList();

                if (!toInsert.Any())
                {
                    logger?.LogDebug("RoleSeeder: all default roles already exist.");
                    return;
                }

                // Insert missing roles inside a transaction
                await using var trx = await db.Database.BeginTransactionAsync(ct);
                db.Roles.AddRange(toInsert);
                await db.SaveChangesAsync(ct);
                await trx.CommitAsync(ct);

                logger?.LogInformation("RoleSeeder: added roles: {roles}", string.Join(", ", toInsert.Select(r => r.Name)));
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "RoleSeeder: failed to seed roles.");
                // swallow so app startup is not blocked; rethrow if you prefer to fail startup.
            }
        }
    }
}
