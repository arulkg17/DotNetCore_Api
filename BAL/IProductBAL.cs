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
        Task<string> DeleteProduct(int id);

    }
}
