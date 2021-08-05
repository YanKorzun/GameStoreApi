using GameStore.BL.Constants;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.DAL.Enums;
using GameStore.DAL.Repositories;
using GameStore.WEB.DTO.ProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.WEB.Constants;

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

        /// <summary>
        /// Get some the most popular platforms
        /// </summary>
        /// <returns>Returns list of product platforms</returns>
        /// <response code="200">Data successfully taken from a database</response>
        [AllowAnonymous]
        [HttpGet("top-platforms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductPlatforms>>> GetMostPopularPlatforms() => Ok(await _productRepository.GetPopularPlatformsAsync(ProductConstants.TopPlatformCount));

        /// <summary>
        /// Filter products by their names, skipped count and offset from the first element
        /// </summary>
        /// <returns>Returns list of product models</returns>
        /// <response code="200">All products received</response>
        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ExtendedProductModel>>> GetProductsByTerm([FromQuery, BindRequired] string term, int limit, int offset) =>
            Ok(await _productService.GetProductsBySearchTermAsync(term, limit, offset));

        /// <summary>
        /// Get full information about product via its id
        /// </summary>
        /// <returns>Returns full product properties model</returns>
        /// <response code="200">Product successfully received</response>
        [AllowAnonymous]
        [HttpGet("id/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ExtendedProductModel>> GetProductById(int id)
        {
            return Ok(await _productService.FindProductById(id));
        }

        /// <summary>
        /// Create product with provided model properties
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///         "name": "string",
        ///         "developers": "string",
        ///         "publishers": "string",
        ///         "genre": "string",
        ///         "rating": 0,
        ///         "logo": "string",
        ///         "background": "string",
        ///         "price": 0,
        ///         "count": 0,
        ///         "dateCreated": "2021-08-05T08:13:56.083Z",
        ///         "totalRating": 0,
        ///         "platform": 0,
        ///         "publicationDate": "2021-08-05T08:13:56.083Z"
        ///     }
        ///
        /// </remarks>
        /// <param name="productModel">data transfer object for creating a new product in database</param>
        /// <response code="201">Created successfully</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User has no access to this resource</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ProductModel>> CreateNewProduct([FromForm] InputProductModel productModel)
        {
            var createdProduct = await _productService.CreateProductAsync(productModel);

            return CreatedAtAction(nameof(CreateNewProduct), createdProduct);
        }

        /// <summary>
        /// Updates product with provided model properties
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///         "name": "string",
        ///         "developers": "string",
        ///         "publishers": "string",
        ///         "genre": "string",
        ///         "rating": 0,
        ///         "logo": "string",
        ///         "background": "string",
        ///         "price": 0,
        ///         "count": 0,
        ///         "dateCreated": "2021-08-05T08:13:56.083Z",
        ///         "totalRating": 0,
        ///         "platform": 0,
        ///         "publicationDate": "2021-08-05T08:13:56.083Z",
        ///         "id": 0
        ///     }
        ///
        /// </remarks>
        /// <param name="basicProductModel">data transfer object for updating existing product in database</param>
        /// <response code="200">Updated successfully</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User has no access to this resource</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ExtendedProductModel>> UpdateProduct([FromForm] ExtendedInputProductModel basicProductModel)
        {
            await _productService.UpdateProductAsync(basicProductModel);

            return Ok();
        }

        /// <summary>
        /// Mark product as deleted in database
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
    }
}