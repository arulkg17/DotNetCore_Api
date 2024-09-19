using BOL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    
    public class ProductDAL:IProductDAL
    {
        private readonly ProductDbContext _context;
        public ProductDAL(ProductDbContext context)
        { 
            _context = context;
        }
        public async Task<List<ProductObj>> GetActiveProducts()
        {
            try
            {
                return await _context.Products
                        .Where(p => p.IsActive)
                        .OrderByDescending(p => p.PostedDate)
                        .ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<ProductObj> AddProduct(ProductObj product)
        {
            try
            {
                var obj = await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return obj.Entity;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<ProductObj> UpdateProduct(ProductObj product)
        {
            try
            {
                var obj = _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return obj.Entity;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<string> DeleteProduct(int id)
        {
            string retVal = string.Empty;

            try
            {
                var product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
                if (product == null) return "Product not found!";
                product.IsActive = false;
                await _context.SaveChangesAsync();
                retVal = "Product deleted successfully!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public async Task<ProductObj> GetProductById(int id)
        {
            
            try
            {
                var obj= await _context.Products.FindAsync(id);
                
                return obj;
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}
