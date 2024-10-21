
namespace BOL
{
    public class ApprovalQueueDto
    {
        public required string ProductName { get; set; }    
        public required string RequestReason { get; set; }  
        public DateTime RequestDate { get; set; }  
    }
}

