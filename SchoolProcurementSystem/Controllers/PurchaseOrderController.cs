using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;

namespace SchoolProcurement.Api.Controllers
{
    [ApiController]
    [Route("api/purchase-orders")]
    [Authorize]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;

        public PurchaseOrderController(IPurchaseOrderService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct = default)
        {
            return Ok(await _service.GetByIdAsync(id, ct));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 25,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            return Ok(await _service.GetPagedAsync(page, pageSize, search, ct));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePurchaseOrderDto dto, CancellationToken ct)
            => Ok(await _service.CreateAsync(dto, ct));

        [HttpPost("{id}/receive")]
        public async Task<IActionResult> Receive(int id, ReceivePurchaseOrderDto dto, CancellationToken ct)
        {
            dto.PurchaseOrderID = id;
            return Ok(await _service.ReceiveAsync(dto, ct));
        }

        [HttpPost("{id}/payments")]
        public async Task<IActionResult> AddPayment(int id, AddPurchaseOrderPaymentDto dto, CancellationToken ct)
        {
            dto.PurchaseOrderID = id;
            return Ok(await _service.AddPaymentAsync(dto, ct));
        }
    }
}
