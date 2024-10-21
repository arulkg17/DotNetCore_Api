using BOL;

namespace DAL
{
    public interface IApprovalQueueDAL
    {
       Task<int> AddApprovalQueue(ProductObj productObj, ApprovalReason reason);
        Task<List<ApprovalQueueObj>> GetApprovalQueues();
        Task<string> ApproveProduct(int approvalId, bool isApproved);

    }
}
