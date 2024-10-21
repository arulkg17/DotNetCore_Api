using BOL;

namespace BAL
{
    public interface IProductBAL
    {
        Task<List<ProductObj>> GetActiveProducts();
        Task<ProductObj> AddProduct(ProductObj product);
        Task<ProductObj> UpdateProduct(ProductObj product);
       // Task<string> DeleteProduct(int id);
        Task<List<ProductObj>> SearchProducts(string? name, decimal? minPrice, decimal? maxPrice, DateTime? startDate, DateTime? endDate);
        Task<string> RequestProductDeletion(int productId);
        Task<string> ApproveProductDeletion(int productId);
        Task<string> RejectProductDeletion(int productId);



    }
}
