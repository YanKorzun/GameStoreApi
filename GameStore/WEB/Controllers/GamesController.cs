﻿using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BL.Constants;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.Utilities;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;
using GameStore.WEB.Constants;
using GameStore.WEB.DTO.Parameters;
using GameStore.WEB.DTO.Products;
using GameStore.WEB.DTO.Ratings;
using GameStore.WEB.Filters.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GameStore.WEB.Controllers
{
    [ApiController]
    [Route("api/games")]
    [Authorize(Roles = UserRoleConstants.Admin)]
    public class GamesController : ControllerBase
    {
        private readonly IProductRatingService _productRatingService;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;

        public GamesController(IProductRepository productRepository, IProductService productService,
            IProductRatingService productRatingService)
        {
            _productRepository = productRepository;
            _productService = productService;
            _productRatingService = productRatingService;
        }

        /// <summary>
        ///     Get some the most popular platforms
        /// </summary>
        /// <returns>Returns list of product platforms</returns>
        /// <response code="200">Data successfully taken from a database</response>
        [AllowAnonymous]
        [HttpGet("top-platforms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductPlatforms>>> GetMostPopularPlatforms() =>
            Ok(await _productRepository.GetPopularPlatformsAsync(ProductConstants.TopPlatformCount));

        /// <summary>
        ///     Filter products by their names, skipped count and offset from the first element
        /// </summary>
        /// <returns>Returns list of product models</returns>
        /// <response code="200">All products received</response>
        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ExtendedProductDto>>> GetProductsByTerm(
            [FromQuery] [BindRequired] string term, int limit, int offset) =>
            await _productService.GetProductsBySearchTermAsync(term, limit, offset);

        /// <summary>
        ///     Get full information about product via its id
        /// </summary>
        /// <returns>Returns full product properties model</returns>
        /// <response code="200">Product successfully received</response>
        /// <response code="404">Product doesn't exist</response>
        [AllowAnonymous]
        [HttpGet("id/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExtendedProductDto>> GetProductById(int id) =>
            await _productService.FindProductByIdAsync(id);

        /// <summary>
        ///     Create product with provided model properties
        /// </summary>
        /// <param name="productDto">data transfer object for creating a new product in database</param>
        /// <response code="201">Created successfully</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User has no access to this resource</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateNewProduct([FromForm] InputProductDto productDto)
        {
            var createProductResult = await _productService.CreateProductAsync(productDto);
            if (createProductResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)createProductResult.Result, createProductResult.ErrorMessage);
            }

            return CreatedAtAction(nameof(CreateNewProduct), createProductResult.Data);
        }

        /// <summary>
        ///     Create rating with provided model properties
        /// </summary>
        /// <param name="ratingDto">data transfer object for creating a new product in database</param>
        /// <response code="201">Created successfully</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User has no access to this resource</response>
        [HttpPost("rating")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRating([FromBody] RatingDto ratingDto)
        {
            var userIdSearchResult = ClaimsUtility.GetUserIdFromClaims(User);
            if (userIdSearchResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)userIdSearchResult.Result, ExceptionMessageConstants.MissingUser);
            }

            var result = await _productRatingService.CreateProductRatingAsync(userIdSearchResult.Data, ratingDto);
            return CreatedAtAction(nameof(CreateRating), result);
        }

        /// <summary>
        ///     Updates product with provided model properties
        /// </summary>
        /// <param name="basicProductDto">data transfer object for updating existing product in database</param>
        /// <response code="200">Updated successfully</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User has no access to this resource</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ExtendedProductDto>> UpdateProduct(
            [FromForm] ExtendedInputProductDto basicProductDto)
        {
            var updateResult = await _productService.UpdateProductAsync(basicProductDto);
            if (updateResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)updateResult.Result, updateResult.ErrorMessage);
            }

            return Ok();
        }

        /// <summary>
        ///     Mark product as deleted in database
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>No content</returns>
        /// <response code="204">Product marked successfully</response>
        /// <response code="400">Problems with updating entity or saving changes in database</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User has no access to this resource</response>
        [HttpDelete("id/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleteResult = await _productService.DeleteProductAsync(id);
            if (deleteResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)deleteResult.Result, deleteResult.ErrorMessage);
            }

            return NoContent();
        }

        /// <summary>
        ///     Get paged list of products from the database
        /// </summary>
        /// <param name="productParametersDto">Provided parameters model</param>
        /// <returns></returns>
        /// <response code="200">Products paged successfully</response>
        [HttpGet("list")]
        [AllowAnonymous]
        [ServiceFilter(typeof(ProductFilter))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductList([FromQuery] ProductParametersDto productParametersDto)
        {
            var products = await _productService.GetPagedProductListAsync(productParametersDto);

            return Ok(products);
        }
    }
}