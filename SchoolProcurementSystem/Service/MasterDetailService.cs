using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure;
using SchoolProcurement.Infrastructure.Persistence;
using SchoolProcurement.Infrastructure.Services;

namespace SchoolProcurement.Api.Service
{
    public class MasterDetailService : BaseService, IMasterDetailService
    {
        private readonly SchoolDbContext _db;

        public MasterDetailService(
            SchoolDbContext db,
            ILogger<MasterDetailService> logger)
            : base(logger)
        {
            _db = db;
        }

        // ----------------------------------------------------
        // Get master details by category (case & space insensitive)
        // ----------------------------------------------------
        public async Task<GeneraicResponse> GetByCategoryAsync(string category, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(category))
                    return new List<MasterDetail>();

                var normalized = Normalize(category);

                var list = await _db.MasterDetails
                    .AsNoTracking()
                    .Where(x =>
                        (x.IsDeleted == false || x.IsDeleted == null) &&
                        x.Category != null &&
                        x.Category.Replace(" ", "").ToLower() == normalized)
                    .OrderBy(x => x.Name)
                    .ToListAsync(ct);

                return list;
            },
            "Master details fetched successfully",
            "Failed to fetch master details");
        }

        // ----------------------------------------------------
        // Helpers
        // ----------------------------------------------------
        private static string Normalize(string value)
        {
            return value
                .Replace(" ", "")
                .Trim()
                .ToLower();
        }
    }
}
