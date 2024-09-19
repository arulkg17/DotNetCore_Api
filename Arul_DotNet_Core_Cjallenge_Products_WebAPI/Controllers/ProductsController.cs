using BAL;
using BOL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace DotNetCore_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductBAL _productBAL;
        private readonly IApprovalQueueBAL _approvalQueueBAL;

        public ProductsController(IProductBAL productBAL, IApprovalQueueBAL approvalQueueBAL)
        { 
            _productBAL = productBAL;
            _approvalQueueBAL = approvalQueueBAL;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveProducts()
        {
            var products = await _productBAL.GetActiveProducts();
            return Ok(products);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductObj product)
        { 
            await _productBAL.AddProduct(product);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductObj product)
        {
            await _productBAL.UpdateProduct(product);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        { 
            await _productBAL.DeleteProduct(id);
            return Ok();
        }
        [HttpGet("approval")]
        public async Task<IActionResult> GetApprovalQueue()
        { 
            var queue = await _approvalQueueBAL.GetApprovalQueues();
            return Ok(queue);
        }
        [HttpPost("approval/{id}/approve")]
        public async Task<IActionResult> ApproveProduct(int id)
        { 
            await _approvalQueueBAL.ApproveProduct(id);
            return Ok();
        }
    }
}
