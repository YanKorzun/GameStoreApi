using GameStore.DAL.Entities;
using GameStore.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.WEB.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public GamesController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("top-platforms")]
        [AllowAnonymous]
        public async Task<ActionResult<IList<Product>>> GetMostPopularPlatforms()
        {
            var platforms = await _productRepository.GetPopularPlatforms();
            return Ok(platforms.Data);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IList<Product>>> GetMostPopularPlatforms([FromQuery, BindRequired] string term, int limit, int offset)
        {
            var platforms = await _productRepository.GetProductsBySearchTerm(term, limit, offset);
            return Ok(platforms);
        }
    }
}