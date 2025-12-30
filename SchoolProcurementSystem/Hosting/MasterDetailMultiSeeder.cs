using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure.Persistence;

namespace SchoolProcurement.Api.Hosting
{
    public static class MasterDetailMultiSeeder
    {
        // NOTE: keys must be unique. No duplicate category names here.
        private static readonly Dictionary<string, string[]> DefaultMasterCategories = new()
        {
            {
                "UnitType", new[]
                {
                    "Pieces",
                    "Boxes",
                    "Units",
                    "Sets",
                    "Kilograms",
                    "Liters",
                    "Meters",
                    "Hours"
                }
            },
            {
                "Urgency Level", new[]
                {
                    "Low - Within 30 days",
                    "Medium - Within 15 days",
                    "High - Within 7 days",
                    "Urgent - Within 3 days"
                }
            },
            {
                "Department", new[]
                {
                    "Information Technology",
                    "Finance & Accounting",
                    "Human Resources",
                    "Operations",
                    "Maintenance",
                    "Administration"
                }
            }
        };

        public static async Task SeedAsync(IServiceProvider services, ILogger? logger = null, CancellationToken ct = default)
        {
            try
            {
                var config = services.GetRequiredService<IConfiguration>();

                var conn = config.GetConnectionString("DefaultConnection")
                           ?? Environment.GetEnvironmentVariable("SP_DEFAULT_CONNECTION")
                           ?? throw new Exception("No DB connection string found.");

                var options = new DbContextOptionsBuilder<SchoolDbContext>()
                    .UseSqlServer(conn)
                    .Options;

                await using var db = new SchoolDbContext(options);

                if (!await db.Database.CanConnectAsync(ct))
                {
                    logger?.LogWarning("MasterDetailMultiSeeder: Cannot connect to database.");
                    return;
                }

                foreach (var categoryPair in DefaultMasterCategories)
                {
                    var category = categoryPair.Key;
                    var values = categoryPair.Value;

                    // Get existing entries (case-insensitive)
                    var existing = await db.MasterDetails
                        .Where(x => x.Category == category && (x.IsDeleted == false || x.IsDeleted == null))
                        .Select(x => x.Name!)
                        .ToListAsync(ct);

                    var existingSet = new HashSet<string>(existing, StringComparer.OrdinalIgnoreCase);

                    var toInsert = values
                        .Where(v => !existingSet.Contains(v))
                        .Select(v => new MasterDetail
                        {
                            Category = category,
                            Name = v,
                            OtherName = string.Empty,
                            Description = string.Empty,
                            IsDeleted = false,
                            ParentID = 0,
                            CreatedDate = DateTime.UtcNow,
                            UpdatedDate = DateTime.UtcNow,
                            CreatedBy = 1,
                            UpdatedBy = 1
                        })
                        .ToList();

                    if (toInsert.Any())
                    {
                        await using var trx = await db.Database.BeginTransactionAsync(ct);

                        db.MasterDetails.AddRange(toInsert);
                        await db.SaveChangesAsync(ct);
                        await trx.CommitAsync(ct);

                        logger?.LogInformation(
                            "MasterDetailMultiSeeder: Added category '{category}' values: {items}",
                            category,
                            string.Join(", ", toInsert.Select(x => x.Name))
                        );
                    }
                    else
                    {
                        logger?.LogInformation(
                            "MasterDetailMultiSeeder: All values already exist for category '{category}'",
                            category
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "MasterDetailMultiSeeder: Error while seeding.");
            }
        }
    }
}
