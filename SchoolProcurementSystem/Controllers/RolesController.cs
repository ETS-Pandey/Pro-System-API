using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _service;
        public RolesController(IRoleService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct = default) => Ok(await _service.GetAllAsync(ct));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct = default)
        {
            return Ok(await _service.GetByIdAsync(id, ct));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to validate the role details."
                });

            return Ok(await _service.CreateAsync(dto, ct));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to validate the role details."
                });

            if (id != dto.ID)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to get the id in the request model."
                });

            return Ok(await _service.UpdateAsync(dto, ct));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            return Ok(await _service.DeleteAsync(id, ct));
        }
    }
}
