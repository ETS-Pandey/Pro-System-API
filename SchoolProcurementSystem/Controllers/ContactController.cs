using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Controllers
{
    [ApiController]
    [Route("api/vendor")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _svc;
        private readonly ISorContactMappingService _sorContactMappingService;

        public ContactController(IContactService svc, ISorContactMappingService sorContactMappingService, ILogger<ContactController> logger)
        {
            _svc = svc;
            _sorContactMappingService = sorContactMappingService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct = default)
        {
            return Ok(await _svc.GetByIdAsync(id, ct));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 25,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            return Ok(await _svc.GetPagedAsync(page, pageSize, search, ct));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to validate the contact details."
                });

            return Ok(await _svc.CreateAsync(dto, ct));
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateContactDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to validate the contact details."
                });

            if (id != dto.ID)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to get the id in the request model."
                });

            var updated = await _svc.UpdateAsync(dto, ct);
            return Ok(updated);
        }

        [HttpGet("Authanticate")]
        public async Task<IActionResult> AuthanticateStep(
            [FromQuery] string email = null,
            [FromQuery] int mapid = 0,
            CancellationToken ct = default)
        {
            return Ok(await _sorContactMappingService.GenerateOTP(email, mapid, ct));
        }

        //[HttpGet("Send/Quotation")]
        //public async Task<IActionResult> SendQuotationEmail(
        //    List<int> contacts, int sorid,
        //    CancellationToken ct = default)
        //{
        //    return Ok(await _sorContactMappingService.SendQuotationEmail(contacts, sorid, ct));
        //}

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            return Ok(await _svc.DeleteAsync(id, ct));
        }
    }
}
