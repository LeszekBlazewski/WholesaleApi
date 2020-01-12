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
    [Authorize(Roles = Role.Client)]
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductsController(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }


        [Authorize(Roles = Role.Employee)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ProductDto dto)
        {
            try
            {
                var model = _mapper.Map<Product>(dto);
                await _service.Create(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var models = await _service.GetAll();
                var dtos = _mapper.Map<IList<ProductDto>>(models);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var model = await _service.GetById(id);
                var dto = _mapper.Map<ProductDto>(model);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [Authorize(Roles = Role.Employee)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]ProductDto dto)
        {
            try
            {
                var model = _mapper.Map<Product>(dto);
                await _service.Update(model);
                return Ok();
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

        [Authorize(Roles = Role.Employee)]
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var stats = await _service.GetStats();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }
    }
}
