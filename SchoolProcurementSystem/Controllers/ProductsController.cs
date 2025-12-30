using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _svc;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService svc, ILogger<ProductsController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        /// <summary>Get paginated list of products. Optional filters: categoryId, unitTypeId, search (name).</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 25,
            [FromQuery] int? categoryId = null,
            [FromQuery] int? unitTypeId = null,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            // result can include total count & items (service should return a DTO). Here we assume it returns (IEnumerable<ProductDto>, totalCount).
            return Ok(await _svc.GetAllPagedAsync(page, pageSize, categoryId, unitTypeId, search, ct));
        }

        /// <summary>Get single product by id</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct = default)
        {
            return Ok(await _svc.GetByIdAsync(id, ct));
        }

        /// <summary>Create a new product</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to validate the product details."
                });

            return Ok(await _svc.CreateAsync(new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                CategoryID = dto.CategoryID,
                UnitTypeID = dto.UnitTypeID,
                SalesPrice = dto.SalesPrice,
                PurchasePrice = dto.PurchasePrice,
                CreatedBy = dto.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = dto.CreatedBy,
                UpdatedDate = DateTime.UtcNow,
                IsDeleted = false
            }, ct));
        }

        /// <summary>Update an existing product</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to validate the product details."
                });

            if (id != dto.ID)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to get the id in the request model."
                });

            return Ok(await _svc.UpdateAsync(new Product
            {
                ID = dto.ID,
                Name = dto.Name,
                Description = dto.Description,
                CategoryID = dto.CategoryID,
                UnitTypeID = dto.UnitTypeID,
                SalesPrice = dto.SalesPrice,
                PurchasePrice = dto.PurchasePrice,
                UpdatedBy = dto.UpdatedBy,
                UpdatedDate = DateTime.UtcNow
            }, ct));
        }

        /// <summary>Soft delete a product</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            return Ok(await _svc.DeleteAsync(id, ct));
        }
    }
}
