using BOL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    
    public class ProductDAL:IProductDAL
    {
        private readonly ProductDbContext _context;
        public ProductDAL(ProductDbContext context)
        { 
            _context = context;
        }
        public async Task<List<ProductObj>> GetActiveProducts()
        {
            try
            {
                return await _context.Products
                        .Where(p => p.IsActive)
                        .OrderByDescending(p => p.PostedDate)
                        .ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<ProductObj> AddProduct(ProductObj product)
        {
            try
            {
                var obj = await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return obj.Entity;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<ProductObj> UpdateProduct(ProductObj product)
        {
            try
            {
                var obj = _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return obj.Entity;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //public async Task<string> DeleteProduct(int id)
        //{
        //    string retVal = string.Empty;

        //    try
        //    {
        //        var product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
        //        if (product == null) return "Product not found!";
        //        product.IsActive = false;
        //        await _context.SaveChangesAsync();
        //        retVal = "Product deleted successfully!";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return retVal;
        //}

        public async Task<ProductObj> GetProductById(int id)
        {
            
            try
            {
                var obj= await _context.Products.FindAsync(id);
                
                return obj;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<List<ProductObj>> SearchProducts(string? name, decimal? minPrice, decimal? maxPrice, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var query = _context.Products.AsQueryable();

                // Filter by name
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Name.Contains(name));
                }

                // Filter by price range
                if (minPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= maxPrice.Value);
                }

                // Filter by posted date range
                if (startDate.HasValue)
                {
                    query = query.Where(p => p.PostedDate >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(p => p.PostedDate <= endDate.Value);
                }

                // Only active products and order by latest first
                return await query.Where(p => p.IsActive)
                                  .OrderByDescending(p => p.PostedDate)
                                  .ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> RequestProductDeletion(int productId)
        {
            string retVal = string.Empty;
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null) return "Product not found.";

                // Create an entry in the approval queue for product deletion
                var approvalRequest = new ApprovalQueueObj
                {
                    ProductId = product.Id,
                    RequestDate = DateTime.UtcNow,
                    Reason = ApprovalReason.ProductDeletion
                };

                // Add to the approval queue
                _context.ApprovalQueues.Add(approvalRequest);

                // Optionally, mark the product as "Pending Deletion" or keep it active
                product.Status = ProductStatus.PendingDeletion;

                await _context.SaveChangesAsync();
                retVal = "Sent to product delete approval";

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return retVal;
        }

        // Method to approve the deletion
        public async Task<string> ApproveProductDeletion(int productId)
        {
            string retVal = string.Empty;
            try
            {

                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                    return "Product not found.";

                _context.Products.Remove(product);

                var approvalRequest = await _context.ApprovalQueues.FirstOrDefaultAsync(aq => aq.ProductId == productId && aq.Reason == ApprovalReason.ProductDeletion);
                if (approvalRequest != null)
                {
                    _context.ApprovalQueues.Remove(approvalRequest);
                }

                await _context.SaveChangesAsync();
                retVal = "Product has been deleted successfully!";
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return retVal;
        }

        // Method to reject product deletion
        public async Task<string> RejectProductDeletion(int productId)
        {
            string retVal = string.Empty;
            try
            {

                var product = await _context.Products.FindAsync(productId);
                if (product == null) return "Product not found.";

                // If rejected, change the product status back to active or its original state
                product.Status = ProductStatus.Active;

                // Remove the approval request from the queue
                var approvalRequest = await _context.ApprovalQueues.FirstOrDefaultAsync(aq => aq.ProductId == productId && aq.Reason == ApprovalReason.ProductDeletion);
                if (approvalRequest != null)
                {
                    _context.ApprovalQueues.Remove(approvalRequest);
                }

                await _context.SaveChangesAsync();
                retVal = "Product deletion rejected and Active";
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return retVal;
        }



    }
}
