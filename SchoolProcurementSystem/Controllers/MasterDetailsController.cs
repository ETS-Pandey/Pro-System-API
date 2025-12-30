using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProcurement.Api.Service.Interface;

namespace SchoolProcurement.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MasterDetailsController : ControllerBase
    {
        private readonly IMasterDetailService _service;

        public MasterDetailsController(IMasterDetailService service)
        {
            _service = service;
        }

        [HttpGet("category/{categoryName}")]
        public async Task<IActionResult> GetByCategory(string categoryName, CancellationToken ct = default)
        {
            return Ok(await _service.GetByCategoryAsync(categoryName, ct));
        }
    }
}
