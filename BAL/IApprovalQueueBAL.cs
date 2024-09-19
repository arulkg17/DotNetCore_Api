using DAL;
using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public interface IApprovalQueueBAL
    {
        Task<int> AddApprovalQueue(ProductObj productObj, ApprovalReason reason);
        Task<List<ApprovalQueueObj>> GetApprovalQueues();
        Task<string> ApproveProduct(int approvalId, bool isApproved);

    }
}
