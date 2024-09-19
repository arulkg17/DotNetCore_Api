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

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<int> ApproveProduct(int approvalId)
        {
            int retVal = 0;

            try
            {
                var approval = await _context.ApprovalQueues.FindAsync(approvalId);
                if (approval == null)
                {
                    throw new Exception("Approval request not found");
                }
                var product = await _context.Products.FindAsync(approval.ProductId);

                if (product != null)
                {
                    product.Status = approval.Reason switch
                    {
                        ApprovalReason.PriceAboveLimit => ProductStatus.Created,
                        ApprovalReason.PriceIncreaseAboveThreshold => ProductStatus.Updated,
                        ApprovalReason.ProductDeletion => ProductStatus.Deleted
                    };

                    //switch (approval.Reason)
                    //{
                    //    case ApprovalReason.PriceAboveLimit:

                    //        product.Status = ProductStatus.Created;
                    //        break;

                    //    case ApprovalReason.PriceIncreaseAboveThreshold:
                    //        product.Status = ProductStatus.Updated;
                    //        break;

                    //    case ApprovalReason.ProductDeletion:

                    //        product.Status = ProductStatus.Deleted;
                    //        break;
                    //    default:
                    //        product.Status = ProductStatus.Created;
                    //        break;
                    //}
                }
                _context.Products.Update(product);
                _context.ApprovalQueues.Remove(approval);
                await _context.SaveChangesAsync();
                retVal = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

      
    }
}
