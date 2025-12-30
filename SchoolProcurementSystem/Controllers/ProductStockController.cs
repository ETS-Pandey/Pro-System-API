using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStockController : ControllerBase
    {
        private readonly IProductStockService _svc;
        private readonly ILogger<ProductStockController> _logger;

        public ProductStockController(IProductStockService svc, ILogger<ProductStockController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetByProduct(int productId, CancellationToken ct = default)
        {
            return Ok(await _svc.GetByProductAsync(productId, ct));
        }

        [HttpGet("branch/{branchId:int}")]
        public async Task<IActionResult> GetByBranch(int branchId, CancellationToken ct = default)
        {
            return Ok(await _svc.GetByBranchAsync(branchId, ct));
        }

        [HttpGet("single")]
        public async Task<IActionResult> GetSingle([FromQuery] int productId, CancellationToken ct = default)
        {
            return Ok(await _svc.GetAsync(productId, ct));
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 25,
            [FromQuery] int? productId = null, [FromQuery] int? branchId = null, [FromQuery] string? search = null, CancellationToken ct = default)
        {
            return Ok(await _svc.GetPagedAsync(page, pageSize, productId, branchId, search, ct));
        }

        [HttpPost("adjust")]
        public async Task<IActionResult> Adjust([FromBody] ProductStockAdjustmentDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to validate the product details."
                });

            return Ok(await _svc.AdjustStockAsync(dto, ct));
        }

        [HttpGet("low/{branchId:int}")]
        public async Task<IActionResult> GetLowStock(CancellationToken ct = default)
        {
            return Ok(await _svc.GetLowStockAsync(ct));
        }
    }
}
