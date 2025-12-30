using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;

namespace SchoolProcurement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        public UsersController(IUserService service) => _service = service;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
            => Ok(await _service.GetAllAsync(page, pageSize, ct));

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct = default)
        {
            return Ok(await _service.GetByIdAsync(id, ct));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken ct = default)
        {
            return Ok(await _service.CreateAsync(dto, ct));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto, CancellationToken ct = default)
        {
            if (id != dto.ID)
                return BadRequest("ID mismatch");

            return Ok(await _service.UpdateAsync(dto, ct));
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            return Ok(await _service.DeleteAsync(id, ct));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct = default)
        {
            return Ok(await _service.LoginAsync(req, ct));
        }
    }
}
