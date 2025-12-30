using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Controllers
{
    [ApiController]
    [Route("api/sor-quotations")]
    public class SorQuotationController : ControllerBase
    {
        private readonly ISorContactMappingService _service;

        public SorQuotationController(ISorContactMappingService service)
        {
            _service = service;
        }

        [HttpPost("invite")]
        public async Task<IActionResult> Invite(
            InviteQuotationDto dto,
            CancellationToken ct)
            => Ok(await _service.InviteContactsAsync(dto, ct));

        [HttpPost("submit")]
        [AllowAnonymous]
        public async Task<IActionResult> Submit(
            SubmitQuotationDto dto,
            CancellationToken ct)
            => Ok(await _service.SubmitQuotationAsync(dto, ct));

        [HttpPost("approve-items")]
        public async Task<IActionResult> ApproveItems(
            ApproveQuotationItemsDto dto,
            CancellationToken ct)
            => Ok(await _service.ApproveQuotationItemsAsync(dto, ct));

        [HttpGet("by-sor/{sorId}")]
        public async Task<IActionResult> GetBySor(int sorId, CancellationToken ct)
            => Ok(await _service.GetBySorAsync(sorId, ct));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct)
            => Ok(await _service.GetByIdAsync(id, ct));
    }

}
