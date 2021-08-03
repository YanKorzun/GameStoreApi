using GameStore.BL.Constants;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Repositories;
using GameStore.WEB.Constants;
using GameStore.WEB.DTO.ProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.WEB.Controllers
{
    [ApiController]
    [Route("api/games")]
    [Authorize(Roles = UserRoleConstants.Admin)]
    public class GamesController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;

        public GamesController(IProductRepository productRepository, IProductService productService)
        {
            _productRepository = productRepository;
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet("top-platforms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<Product>>> GetMostPopularPlatforms() => Ok(await _productRepository.GetPopularPlatformsAsync(ProductConstants.TopPlatformCount));

        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<Product>>> GetProductsByTerm([FromQuery, BindRequired] string term, int limit, int offset) =>
            Ok(await _productRepository.GetProductsBySearchTermAsync(term, limit, offset));

        [AllowAnonymous]
        [HttpGet("id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<Product>>> GetProductById(int id)
        {
            return Ok(await _productRepository.FindProductById(id));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<IList<Product>>> CreateNewProduct(ProductModel productModel)
        {
            var createdProduct = await _productService.CreateNewProductAsync(productModel);

            return CreatedAtAction(nameof(CreateNewProduct), createdProduct);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<Product>>> UpdateProduct(ProductWithIdModel productModel)
        {
            await _productService.UpdateProductAsync(productModel);

            return Ok();
        }

        [HttpDelete("id/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<Product>>> DeleteProduct(int id)
        {
            var deleteResult = await _productService.DeleteProductAsync(id);
            if (deleteResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)deleteResult.Result, deleteResult.ErrorMessage);
            }

            return NoContent();
        }
    }
}