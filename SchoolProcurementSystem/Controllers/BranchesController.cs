using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SchoolProcurement.ApiControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BranchesController : ControllerBase
    {
        private readonly IBranchService _service;

        public BranchesController(IBranchService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            CancellationToken ct = default)
        {
            var branches = await _service.GetAllAsync(page, pageSize, ct);
            return Ok(branches);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct = default)
        {
            var branch = await _service.GetByIdAsync(id, ct);
            return Ok(branch);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBranchDto dto, CancellationToken ct = default)
        {
            var created = await _service.CreateAsync(dto, ct);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBranchDto dto, CancellationToken ct = default)
        {
            if (id != dto.ID)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to get the id in the request model."
                });

            var updated = await _service.UpdateAsync(dto, ct);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var deleted = await _service.DeleteAsync(id, ct);
            return Ok(deleted);
        }
    }
}
