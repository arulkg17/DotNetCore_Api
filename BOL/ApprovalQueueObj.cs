using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class  ApprovalQueueObj
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductObj Product { get; set; }
        public ApprovalReason Reason { get; set; }
        public DateTime RequestDate { get; set; }

    }
}
