using BAL;
using BOL;
using DotNetCore_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;


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

        [HttpGet("activeProducts")]
        public async Task<ActionResult<StandardResponse<List<ProductObj>>>> GetActiveProducts()
        {
            List<ProductObj> products = new List<ProductObj>();
            try
            {
                products = await _productBAL.GetActiveProducts();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
            return new JsonResult(new StandardResponse<IEnumerable<ProductObj>>(true, "Products retrieved successfully", products))
            { 
                StatusCode = 200,
            };
        }
        [HttpPost("create")]
        public async Task<ActionResult<StandardResponse<ProductObj>>> CreateProduct([FromBody] ProductObj product)
        {
            ProductObj productObj = new ProductObj();
            try
            {
                 productObj = await _productBAL.AddProduct(product);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return new JsonResult(new StandardResponse<ProductObj>(true, "Product added successfully", productObj)) { 
            StatusCode = 201
            };
        }
        [HttpPut("update/{id}")]
        public async Task<ActionResult<StandardResponse<ProductObj>>> UpdateProduct(int id, [FromBody] ProductObj product)
        {
            ProductObj productObj = new ProductObj();
            try
            {
                product.Id = id;
                productObj = await _productBAL.UpdateProduct(product);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return new JsonResult(new StandardResponse<ProductObj>(true, "Product updated successfully", productObj))
            {
                StatusCode = 200
            };
           
        }

        [HttpGet("approvals")]
        public async Task<ActionResult<StandardResponse<List<ApprovalQueueObj>>>> GetApprovalQueue()
        {
            List<ApprovalQueueObj> approvalQueues = new List<ApprovalQueueObj>();
            try
            {
                approvalQueues = await _approvalQueueBAL.GetApprovalQueues();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new JsonResult(new StandardResponse<IEnumerable<ApprovalQueueObj>>(true, "Approvals queues retrieved successfully", approvalQueues))
            { 
                StatusCode = 200
            };
        }
        [HttpPost("approval/{id}/approve")]
        public async Task<ActionResult<StandardResponse<string>>> ApproveProduct(int id, [FromQuery] bool isApproved)
        {
           string retVal = string.Empty;
            try
            {
                retVal = await _approvalQueueBAL.ApproveProduct(id, isApproved);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return new JsonResult(new StandardResponse<string>(true, retVal, ""))
            {
                StatusCode = 200
            };
        }

        [HttpGet("search")]
        public async Task<ActionResult<StandardResponse<List<ProductObj>>>> ProductSearch(
            [FromQuery] string? name,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            List<ProductObj> searchResult = new List<ProductObj>();
            try
            {
                searchResult = await _productBAL.SearchProducts(name, minPrice, maxPrice, startDate, endDate);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return new JsonResult(new StandardResponse<List<ProductObj>>(true, "Records retrieved successfully!", searchResult))
            {
                StatusCode = 200
            };
        }

        // API to request product deletion (pushes to approval queue)
        [HttpDelete("approval/{id}/request-deletion")]
        public async Task<ActionResult<StandardResponse<string>>> RequestProductDeletion(int id)
        {
            string retVal = string.Empty;
            try
            {
              retVal = await _productBAL.RequestProductDeletion(id);
            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
            }

            return new JsonResult(new StandardResponse<string>(true, retVal, ""))
            {
                StatusCode = 200
            };
        }

        // API to approve product deletion
        [HttpPost("approval/{id}/approve-deletion")]
        public async Task<ActionResult<StandardResponse<string>>> ApproveProductDeletion(int id)
        {
            string retVal = string.Empty;
            try
            {
                retVal = await _productBAL.ApproveProductDeletion(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new JsonResult(new StandardResponse<string>(true, retVal, ""))
            {
                StatusCode = 200
            };

        }

        // API to reject product deletion
        [HttpPost("approval/{id}/reject-deletion")]
        public async Task<ActionResult<StandardResponse<string>>> RejectProductDeletion(int id)
        {
            string retVal = string.Empty;
            try
            {
                retVal = await _productBAL.RejectProductDeletion(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new JsonResult(new StandardResponse<string>(true, retVal, ""))
            {
                StatusCode = 200
            };

        }

    }
}
