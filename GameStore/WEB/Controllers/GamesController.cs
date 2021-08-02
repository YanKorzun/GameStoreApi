using GameStore.DAL.Entities;
using GameStore.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IList<Product>> GetMostPopularPlatforms() => Ok(_productRepository.GetPopularPlatforms());

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<Product>>> GetMostPopularPlatforms([FromQuery, BindRequired] string term, int limit, int offset) =>
            Ok(await _productRepository.GetProductsBySearchTerm(term, limit, offset));
    }
}