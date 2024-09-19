using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IProductDAL
    {
        Task<List<ProductObj>> GetActiveProducts();
        Task<ProductObj> AddProduct(ProductObj product);
        Task<ProductObj> UpdateProduct(ProductObj product);
        //Task<string> DeleteProduct(int id);
        Task<ProductObj> GetProductById(int id);
        Task<List<ProductObj>> SearchProducts(string? name, decimal? minPrice, decimal? maxPrice, DateTime? startDate, DateTime? endDate);
        Task<string> RequestProductDeletion(int productId);
        Task<string> ApproveProductDeletion(int productId);
        Task<string> RejectProductDeletion(int productId);


    }
}
