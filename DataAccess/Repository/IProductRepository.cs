using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> ListProduct();
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
        Product FindById(int id);
        IEnumerable<Product> MultiSearch(int? productID, string? productName, decimal? unitPrice, int? unitInStock);
        IEnumerable<Product> FindByName(string name);
        IEnumerable<Product> FindByPrice(decimal price);
        IEnumerable<Product> FindByStock(int stock);
    }
}
