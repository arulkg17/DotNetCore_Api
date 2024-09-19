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
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{ 
        //    await _productBAL.DeleteProduct(id);
        //    return Ok();
        //}
        [HttpGet("approval")]
        public async Task<IActionResult> GetApprovalQueue()
        { 
            var queue = await _approvalQueueBAL.GetApprovalQueues();
            return Ok(queue);
        }
        [HttpPost("approval/{id}/approve")]
        public async Task<IActionResult> ApproveProduct(int id, [FromQuery] bool isApproved)
        { 
            await _approvalQueueBAL.ApproveProduct(id,isApproved);
            return Ok();
        }

        [HttpGet("search")]
        public async Task<IActionResult> ProductSearch(
            [FromQuery] string? name,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var products = await _productBAL.SearchProducts(name, minPrice, maxPrice, startDate, endDate);
            return Ok(products);
        }

        // API to request product deletion (pushes to approval queue)
        [HttpDelete("{id}")]
        public async Task<IActionResult> RequestProductDeletion(int id)
        {
            await _productBAL.RequestProductDeletion(id);
            return Ok("Product deletion request submitted for approval.");
        }

        // API to approve product deletion
        [HttpPost("approval/{id}/approve-deletion")]
        public async Task<IActionResult> ApproveProductDeletion(int id)
        {
            await _productBAL.ApproveProductDeletion(id);
            return Ok("Product deletion approved and product removed.");
        }

        // API to reject product deletion
        [HttpPost("approval/{id}/reject-deletion")]
        public async Task<IActionResult> RejectProductDeletion(int id)
        {
            await _productBAL.RejectProductDeletion(id);
            return Ok("Product deletion rejected and product restored.");
        }


    }
}
