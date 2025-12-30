using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SchoolProcurement.Infrastructure.Persistence;

namespace SchoolProcurement.Infrastructure.Persistence.Design
{
    public class SchoolDbContextFactory : IDesignTimeDbContextFactory<SchoolDbContext>
    {
        public SchoolDbContext CreateDbContext(string[] args)
        {
            var config = DesignTimeConfigHelper.GetConfig();

            // Try appsettings.json connection string first
            var conn = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(conn))
            {
                // Final fallback (local dev)
                conn = "Server=localhost;Database=SchoolProcurement;Trusted_Connection=True;TrustServerCertificate=True;";
            }

            var builder = new DbContextOptionsBuilder<SchoolDbContext>();
            builder.UseSqlServer(conn);

            return new SchoolDbContext(builder.Options);
        }
    }
}
