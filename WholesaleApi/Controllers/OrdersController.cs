using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wholesale.BL.Enums;
using Wholesale.BL.Models;
using Wholesale.BL.Models.Dto;
using Wholesale.BL.Models.Query;
using Wholesale.BL.Services;

namespace WholesaleApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [Authorize(Roles = Role.Client)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OrderDto dto)
        {
            try
            {
                var model = _mapper.Map<Order>(dto);
                await _service.Create(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [Authorize(Roles = Role.Employee)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var models = await _service.GetAll();
                var dtos = _mapper.Map<IList<OrderDto>>(models);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [Authorize(Roles = Role.Client)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            try
            {
                var models = await _service.GetByUserId(id);
                var dtos = _mapper.Map<IList<OrderDto>>(models);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [Authorize(Roles = Role.Courier)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UpdateOrderStatusQuery dto)
        {
            try
            {
                var model = _mapper.Map<Order>(dto);
                await _service.Update(model);
                return Ok(_mapper.Map<UpdateOrderStatusQuery>(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [Authorize(Roles = Role.Employee)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [Authorize(Roles = Role.Courier)]
        [HttpGet("available")]
        public async Task<IActionResult> GetAllAvailable()
        {
            try
            {
                var models = await _service.GetAllAvailable();
                return Ok(_mapper.Map<IList<OrderDto>>(models));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [Authorize(Roles = Role.Courier)]
        [HttpGet("courier/{id}")]
        public async Task<IActionResult> GetAllInProgress(int id)
        {
            try
            {
                var models = await _service.GetForCourierByStatus(id, OrderStatus.InProgress);
                return Ok(_mapper.Map<IList<OrderDto>>(models));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [Authorize(Roles = Role.Courier)]
        [HttpGet("completed/{courierId}")]
        public async Task<IActionResult> GetAllCompleted(int courierId)
        {
            try
            {
                var models = await _service.GetForCourierByStatus(courierId, OrderStatus.Completed);
                return Ok(_mapper.Map<IList<OrderDto>>(models));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [Authorize(Roles = Role.Employee)]
        [HttpPost("totalworth")]
        public async Task<IActionResult> GetTotalWorth([FromBody]OrderTotalWorthQuery query)
        {
            try
            {
                var model = await _service.GetOrdersTotalWorth(query.From, query.To);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }
    }
}
