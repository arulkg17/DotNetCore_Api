
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
