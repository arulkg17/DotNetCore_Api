using BOL;

namespace BAL
{
    public interface IApprovalQueueBAL
    {
        Task<int> AddApprovalQueue(ProductObj productObj, ApprovalReason reason);
        Task<List<ApprovalQueueObj>> GetApprovalQueues();
        Task<string> ApproveProduct(int approvalId, bool isApproved);

    }
}
