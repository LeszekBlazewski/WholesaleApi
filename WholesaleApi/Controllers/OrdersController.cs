using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wholesale.BL.Enums;
using Wholesale.BL.Models;
using Wholesale.BL.Models.Dto;
using Wholesale.BL.Services;

namespace WholesaleApi.Controllers
{
    [Authorize(Roles = Role.Employee)]
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
                //foreach (var orderDetail in model.OrderDetails)
                //{
                //    orderDetail.ProductId = orderDetail.Product.ProductId;
                //    orderDetail.Product = null;
                //}
                await _service.Create(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }

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
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
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
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]OrderDto dto)
        {
            try
            {
                var model = _mapper.Map<Order>(dto);
                await _service.Update(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }

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
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
    }
}
