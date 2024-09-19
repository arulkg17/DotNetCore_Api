using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class ApprovalQueueDto
    {
        public required string ProductName { get; set; }    
        public required string RequestReason { get; set; }  
        public DateTime RequestDate { get; set; }  
    }
}

