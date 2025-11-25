using Microsoft.AspNetCore.Mvc;
using ProductManagement.API.DTOs;
using ProductManagement.Application.Events.Interfaces;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.API.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController(IProductRepository productRepository, IProductEventService eventService) : Controller
    {
        IProductRepository _productRepository = productRepository;
        IProductEventService _eventService = eventService;
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult> PostProduct([FromBody] CreateProductRequest request)
        {
            Product product = new Product()
            {
                Name = request.Name,
                Category = request.CategoryName,
                UnitCost = request.UnitCost,
                CreatedAt = DateTime.UtcNow
            };

            await _productRepository.Add(product);
            await _eventService.PublishCreatedProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponse>> GetProduct(int id)
        {
            Product? product = await _productRepository.GetById(id);

            if (product == null)
                return NotFound();

            return ConvertProductOnProductResponse(product);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponse>> PutProduct([FromBody] UpdateProductRequest request, int id)
        {
            Product? product = await _productRepository.GetById(id);

            if (product == null)
                return NotFound();

            product.Name = request.Name ?? product.Name;
            product.Category = request.CategoryName ?? product.Category;
            product.UnitCost = request.UnitCost ?? product.UnitCost;

            await _productRepository.Update(product);
            return ConvertProductOnProductResponse(product);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            Product? product = await _productRepository.GetById(id);

            if (product == null)
                return NotFound();

            await _productRepository.Delete(product);
            return NoContent();
        }
        private static ProductResponse ConvertProductOnProductResponse(Product product)
        {
            return new ProductResponse()
            {
                Name = product.Name,
                CategoryName = product.Category,
                CreatedAt = product.CreatedAt,
                ProductId = product.Id,
                UnitCost = product.UnitCost
            };
        }
    }
}
