using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wholesale.BL.Enums;
using Wholesale.BL.Services;

namespace WholesaleApi.Controllers
{
    [Authorize(Roles = Role.Courier)]
    [ApiController]
    [Route("[controller]")]
    public class CouriersController : ControllerBase
    {
        private readonly ICourierService _courierService;

        public CouriersController(ICourierService courierService)
        {
            _courierService = courierService;
        }

        [Authorize(Roles = Role.Employee)]
        [HttpGet("stats")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var courierStats = await _courierService.GetStatsForAll();
                return Ok(courierStats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [HttpGet("stats/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var courierStats = await _courierService.GetStatsByCourierId(id);
                return Ok(courierStats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }
    }
}
