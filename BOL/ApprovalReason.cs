using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public enum ApprovalReason
    {
        PriceAboveLimit = 1,
        PriceIncreaseAboveThreshold = 2,
        ProductDeletion = 3
    }
}
