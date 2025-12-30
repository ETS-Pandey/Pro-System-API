using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceOrderRequestsController : ControllerBase
    {
        private readonly ISorService _sor;
        private readonly ILogger<ServiceOrderRequestsController> _logger;

        public ServiceOrderRequestsController(ISorService sor, ILogger<ServiceOrderRequestsController> logger)
        {
            _sor = sor;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSorDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to validate the service order request details."
                });

            return Ok(await _sor.CreateAsync(dto, ct));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct = default)
        {
            return Ok(await _sor.GetByIdAsync(id, ct));
        }

        /// <summary>
        /// Get assigned users history for a Service Order Request
        /// </summary>
        [HttpGet("{sorId:int}/assigned-users")]
        public async Task<IActionResult> GetAssignedUsers(int sorId, CancellationToken ct)
        {
            var result = await _sor.GetAssignedUsersBySorIdAsync(sorId, ct);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 25,
            [FromQuery] int? branchId = null, [FromQuery] int? assignedUserId = null, [FromQuery] string? status = null, CancellationToken ct = default)
        {
            return Ok(await _sor.GetPagedAsync(page, pageSize, assignedUserId, status, ct));
        }

        [HttpPost("{id:int}/assign")]
        public async Task<IActionResult> Assign(int id, [FromBody] SorAssignDto dto, CancellationToken ct = default)
        {
            if (id != dto.SORID)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to get the id in the request model."
                });

            return Ok(await _sor.AssignAsync(dto.SORID, dto.ToUserID, dto.Note, ct));
        }

        [HttpPost("{id:int}/reassign")]
        public async Task<IActionResult> Reassign(int id, [FromBody] SorAssignDto dto, CancellationToken ct = default)
        {
            if (id != dto.SORID)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to get the id in the request model."
                });

            return Ok(await _sor.ReassignAsync(dto.SORID, dto.ToUserID, dto.Note, ct));
        }

        [HttpPost("{id:int}/approve")]
        public async Task<IActionResult> Approve(int id, [FromBody] SorApproveDto dto, CancellationToken ct = default)
        {
            if (id != dto.SORID)
                return Ok(new GeneraicResponse()
                {
                    status = "error",
                    message = "Failed to get the id in the request model."
                });

            return Ok(await _sor.ApproveAsync(dto.SORID, dto.Approve, dto.Note, ct));
        }

        [HttpPost("{id:int}/item")]
        public async Task<IActionResult> AddItem(int id, [FromBody] SorItemCreateDto dto, CancellationToken ct = default)
        {
            return Ok(await _sor.AddItemAsync(id, dto, ct));
        }

        [HttpPost("{id:int}/attachments")]
        public async Task<IActionResult> AddAttachment(int id, [FromBody] SorAttachmentCreateDto dto, CancellationToken ct = default)
        {
            return Ok(await _sor.AddAttachmentAsync(id, dto, ct));
        }
    }
}
