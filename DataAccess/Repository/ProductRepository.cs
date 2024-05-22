using BusinessObject.Entity;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        public void Add(Product product)
        {
            ProductDAO.Instance.AddProduct(product);
        }

        public Product FindById(int id)
        {
            return ProductDAO.Instance.FindOne(product => product.ProductId == id);
        }

        public IEnumerable<Product> FindByName(string name)
        {
            return ProductDAO.Instance.FindAll(product => product.ProductName == name);
        }

        public IEnumerable<Product> FindByPrice(decimal price)
        {
            return ProductDAO.Instance.FindAll(product => product.UnitPrice == price);
        }

        public IEnumerable<Product> FindByStock(int stock)
        {
            return ProductDAO.Instance.FindAll(product => product.UnitsInStock == stock);
        }

        public IEnumerable<Product> ListProduct()
        {
            return ProductDAO.Instance.GetProducts();
        }

        public void Delete(Product product)
        {
            ProductDAO.Instance.DeleteProduct(product);
        }

        public void Update(Product product)
        {
            ProductDAO.Instance.UpdateProduct(product);
        }

        public IEnumerable<Product> MultiSearch(int? productID, string? productName, decimal? unitPrice, int? unitInStock)
        {
            return ProductDAO.Instance.FindAll(product =>
                (!productID.HasValue || product.ProductId == productID.Value) &&
                (string.IsNullOrEmpty(productName) || product.ProductName.Contains(productName)) &&
                (!unitPrice.HasValue || product.UnitPrice == unitPrice.Value) &&
                (!unitInStock.HasValue || product.UnitsInStock == unitInStock.Value)
            );
        }
    }
}
