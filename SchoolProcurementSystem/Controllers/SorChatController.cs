using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;

namespace SchoolProcurement.Api.Controllers
{
    [ApiController]
    [Route("api/sor-chat")]
    [Authorize]
    public class SorChatController : ControllerBase
    {
        private readonly ISorChatService _service;

        public SorChatController(ISorChatService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
        [FromForm] SorChatCreateDto dto,
        [FromForm] List<IFormFile>? files,
        CancellationToken ct)
        => Ok(await _service.CreateAsync(dto, files, ct));

        [HttpGet("{sorId}")]
        public async Task<IActionResult> GetBySor(int sorId, CancellationToken ct)
            => Ok(await _service.GetBySorAsync(sorId, ct));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => Ok(await _service.DeleteAsync(id, ct));
    }

}
