using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interface;
using ProductApi.Application.Conversions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductInterface _productInterface;

        public ProductController(IProductInterface productInterface)
        {
            _productInterface = productInterface;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _productInterface.GetAllAsync();
            if (!products.Any()) return NotFound("No products found.");
            return Ok(ProductConversions.ToDTO(products));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _productInterface.FindByIdAsync(id);
            if (product == null) return NotFound($"Product with ID {id} not found.");
            return Ok(ProductConversions.ToDTO(product));
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductDTO productDto)
        {
            if (productDto == null) return BadRequest("Product data is null.");

            var entity = ProductConversions.ToEntity(productDto);
            var created = await _productInterface.CreateAsync(entity);
            return CreatedAtAction(nameof(GetProduct), new { id = created.Id }, ProductConversions.ToDTO(created));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, [FromBody] ProductDTO productDto)
        {
            if (productDto == null || id != productDto.Id) return BadRequest("Invalid product data.");

            var entity = ProductConversions.ToEntity(productDto);
            var updated = await _productInterface.UpdateAsync(entity);
            if (updated == null) return NotFound($"Product with ID {id} not found.");

            return Ok(ProductConversions.ToDTO(updated));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productInterface.FindByIdAsync(id);
            if (product == null) return NotFound($"Product with ID {id} not found.");

            var deleted = await _productInterface.DeleteAsync(product);
            if (!deleted) return StatusCode(500, "Error deleting product.");

            return NoContent();
        }
    }
}
