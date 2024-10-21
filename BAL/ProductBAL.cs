using BOL;
using DAL;

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
            List<ProductObj> products = new List<ProductObj>();
            try
            {
                products =  await _productDAL.GetActiveProducts();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
            return products;
        }
        public async Task<ProductObj> AddProduct(ProductObj product)
        {
            ProductObj obj = new ProductObj();
            try
            {
                if (product.Price > 10000)
                    throw new Exception("Product price cannot exceed $10,000.");

                obj = await _productDAL.AddProduct(product);

                if (product.Price > 5000)
                {
                    // Add to approval queue
                    await _approvalQueueDAL.AddApprovalQueue(product, ApprovalReason.PriceAboveLimit);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
            return obj;
        }
        public async Task<ProductObj> UpdateProduct(ProductObj product)
        {
            ProductObj productObj = new ProductObj();
            try
            {
                var existingProduct = await _productDAL.GetProductById(product.Id);
                if (existingProduct == null)
                    throw new Exception("Product not found.");

                if (product.Price > 5000 || product.Price > (existingProduct.Price * 1.5m))
                {
                    // Add to approval queue
                   await _approvalQueueDAL.AddApprovalQueue(product, ApprovalReason.PriceIncreaseAboveThreshold);
                }

                productObj = await _productDAL.UpdateProduct(product);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return productObj;
        }
        //public async Task<string> DeleteProduct(int id)
        //{
        //    string retVal = string.Empty;
        //    try
        //    {
        //        var product = _productDAL.GetProductById(id);
        //        if (product == null) return "Product not found";

        //        retVal = await _productDAL.DeleteProduct(id);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return retVal;
        //}

        public async Task<List<ProductObj>> SearchProducts(string? name, decimal? minPrice, decimal? maxPrice, DateTime? startDate, DateTime? endDate)
        {
            List<ProductObj> products = new List<ProductObj>();
            try
            {
                products = await _productDAL.SearchProducts(name,minPrice,maxPrice,startDate,endDate);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return products;
        }

        public async Task<string> RequestProductDeletion(int productId) {
            string retVal = string.Empty;
            try
            {
                retVal = await _productDAL.RequestProductDeletion(productId);   
            }
            catch (Exception ex) 
            {

                throw new Exception(ex.Message);
            }
            return retVal;
        }
        public async Task<string> ApproveProductDeletion(int productId)
        {
            string retVal = string.Empty;
            try
            {
                retVal= await _productDAL.ApproveProductDeletion(productId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return retVal;
        }
        public async Task<string> RejectProductDeletion(int productId)
        {
            string retVal = string.Empty;
            try
            {
                retVal =  await _productDAL.RejectProductDeletion(productId);
            }
            catch (Exception ex)
            {

                throw new Exception (ex.Message);
            }
            return retVal;
        }





    }
}
