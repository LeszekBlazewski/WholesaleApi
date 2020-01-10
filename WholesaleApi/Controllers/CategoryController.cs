using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wholesale.BL.Models;
using Wholesale.BL.Models.Dto;
using Wholesale.BL.Services;

namespace WholesaleApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IProductCategoryService _service;
        private readonly IMapper _mapper;

        public CategoryController(IProductCategoryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ProductCategoryDto dto)
        {
            try
            {
                var model = _mapper.Map<ProductCategory>(dto);
                await _service.Create(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _service.GetAll();
                var model = _mapper.Map<IList<ProductCategoryDto>>(users);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _service.GetById(id);
                var model = _mapper.Map<ProductCategoryDto>(user);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]ProductCategoryDto dto)
        {
            try
            {
                var model = _mapper.Map<ProductCategory>(dto);
                await _service.Update(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
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
                return BadRequest(new { ex.Message });
            }
        }
    }
}
