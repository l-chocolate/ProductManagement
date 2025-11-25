using Microsoft.AspNetCore.Mvc;
using ProductManagement.Domain.Entities;

namespace ProductManagement.API.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : Controller
    {
        [HttpPost]
        public Task<ActionResult> PostProduct(Product product)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public Task<ActionResult<IEnumerable<Product>>> GetProduct(int id)
        {
            throw new NotImplementedException();
        }
        [HttpPut]
        Task<ActionResult<Product>> PutProduct(Product product)
        {
            throw new NotImplementedException();
        }
        [HttpDelete]
        Task<ActionResult> DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
