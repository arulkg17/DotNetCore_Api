using DAL;
using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class ApprovalQueueBAL:IApprovalQueueBAL
    {
        private readonly IApprovalQueueDAL _approvalQueueDAL;

        public ApprovalQueueBAL(IApprovalQueueDAL approvalQueueDAL)
        { 
            _approvalQueueDAL = approvalQueueDAL;
        }

        public async Task<int> AddApprovalQueue(ProductObj productObj, ApprovalReason reason)
        {
            try
            {
                return await _approvalQueueDAL.AddApprovalQueue(productObj, reason);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public async Task<List<ApprovalQueueObj>> GetApprovalQueues()
        {
            try
            {
                return await _approvalQueueDAL.GetApprovalQueues();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public async Task<int> ApproveProduct(int approvalId)
        {
            try
            {
                return await _approvalQueueDAL.ApproveProduct(approvalId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

    }
}
