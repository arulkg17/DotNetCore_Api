using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class ProductBAL:IProductBAL
    {
        private readonly IProductDAL _productDAL; 
        private readonly IApprovalQueueDAL _approvalQueueDAL;

        public ProductBAL(IProductDAL productDAL,IApprovalQueueDAL approvalQueueDAL)
        { 
            _productDAL = productDAL;
            _approvalQueueDAL = approvalQueueDAL;
        }

        public async Task<List<ProductObj>> GetActiveProducts()
        {
            try
            {
                return await _productDAL.GetActiveProducts();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public async Task<string> AddProduct(ProductObj product)
        {
            string retVal = string.Empty;
            try
            {
                if (product.Price > 10000)
                    retVal= "Product price cannot exceed $10,000.";
                {
                    ProductObj obj = await _productDAL.AddProduct(product);
                    retVal= "Product has been added successfully!";

                    if (product.Price > 5000)
                    {
                        // Add to approval queue
                        await _approvalQueueDAL.AddApprovalQueue(product, ApprovalReason.PriceAboveLimit);
                        retVal= "Product has been added to Approval Queue!";
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return retVal;
        }
        public async Task<string> UpdateProduct(ProductObj product)
        {
            string retVal = string.Empty;
            try
            {
                var existingProduct = await _productDAL.GetProductById(product.Id);
                if (existingProduct == null)
                    retVal= "Product not found.";

                if (product.Price > 5000 || product.Price > (existingProduct.Price * 1.5m))
                {
                    // Add to approval queue
                    await _approvalQueueDAL.AddApprovalQueue(product, ApprovalReason.PriceIncreaseAboveThreshold);
                    retVal= "Product has been added to Approval Queue!";
                }

                await _productDAL.UpdateProduct(product);
                retVal = "Product bas been updated successfuylly!";
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return retVal;
        }
        public async Task<string> DeleteProduct(int id)
        {
            string retVal = string.Empty;
            try
            {
                var product = _productDAL.GetProductById(id);
                if (product == null) return "Product not found";
                retVal = await _productDAL.DeleteProduct(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return retVal;
        }


    }
}
