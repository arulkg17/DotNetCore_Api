using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public interface IProductBAL
    {
        Task<List<ProductObj>> GetActiveProducts();
        Task<string> AddProduct(ProductObj product);
        Task<string> UpdateProduct(ProductObj product);
       // Task<string> DeleteProduct(int id);
        Task<List<ProductObj>> SearchProducts(string? name, decimal? minPrice, decimal? maxPrice, DateTime? startDate, DateTime? endDate);
        Task<string> RequestProductDeletion(int productId);
        Task<string> ApproveProductDeletion(int productId);
        Task<string> RejectProductDeletion(int productId);



    }
}
