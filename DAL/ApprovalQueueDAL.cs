using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOL;

namespace DAL
{
    public class ApprovalQueueDAL:IApprovalQueueDAL
    {
        private readonly ProductDbContext _context;

        public ApprovalQueueDAL(ProductDbContext context)
        { 
            _context = context;
        }

        public async Task<int> AddApprovalQueue(ProductObj productObj, ApprovalReason reason)
        {
            int retVal = 9;
            try
            {
                var approvalRequest = new ApprovalQueueObj
                {
                    ProductId = productObj.Id,
                    Reason = reason,
                    RequestDate = DateTime.Now,

                };

                await _context.ApprovalQueues.AddAsync(approvalRequest);
                await _context.SaveChangesAsync();
                retVal = 1;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return retVal;

        }
        public async Task<List<ApprovalQueueObj>> GetApprovalQueues()
        {
            try
            {
                return await _context.ApprovalQueues
                        .Include(a => a.Product)
                        .OrderBy(a => a.RequestDate)
                        .ToListAsync();

                //var approvalQueue = await _context.ApprovalQueues
                //                    .Include(aq => aq.Product) // Include product details
                //                    .OrderBy(aq => aq.RequestDate) // Order by oldest first
                //                    .Select(aq => new ApprovalQueueDto
                //                    {
                //                        ProductName = aq.Product.Name,          
                //                        RequestReason = aq.Reason.ToString(),   
                //                        RequestDate = aq.RequestDate            
                //                    })
                //                    .ToListAsync();

                //return approvalQueue;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<string> ApproveProduct(int approvalId, bool isApproved)
        {
            string retVal = string.Empty;

            try
            {
                var approval = await _context.ApprovalQueues.FindAsync(approvalId);
                if (approval == null) return "Approval request not found";
                
                var product = await _context.Products.FindAsync(approval.ProductId);
                if (product == null) return "Product not found";
                if (isApproved)
                {
                    switch (approval.Reason)
                    {
                        case ApprovalReason.PriceAboveLimit:
                        case ApprovalReason.PriceIncreaseAboveThreshold:
                            product.Status = ProductStatus.Updated;
                            product.IsActive = true;
                            retVal = "Product approved successfully";
                            break;
                        case ApprovalReason.ProductDeletion:
                            product.IsActive = false;
                            retVal = "Product rejected successfully";
                            break;
                        default:
                            retVal = "Unknown approval reason.";
                            break;
                    }

                    _context.ApprovalQueues.Remove(approval);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    approval.RequestDate = DateTime.UtcNow;  
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

      
    }
}
