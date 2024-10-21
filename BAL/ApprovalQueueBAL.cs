using DAL;
using BOL;

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

                throw new Exception(ex.Message);
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

                throw new Exception(ex.Message);
            }
            
        }
        public async Task<string> ApproveProduct(int approvalId, bool isApproved)
        {
            try
            {
                return await _approvalQueueDAL.ApproveProduct(approvalId,isApproved);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

    }
}
