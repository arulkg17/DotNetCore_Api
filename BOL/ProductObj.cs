
namespace BOL
{
    public class ProductObj
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsActive { get; set; }
        public ProductStatus Status { get; set; }
    }
}
