using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ProductDAO
    {
        public static ProductDAO instance;
        private static readonly object instanceLock = new object();
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }

        public Product FindOne(Expression<Func<Product, bool>> predicate)
        {
            Product product = null;
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    product = salesDB.Products.Include(or => or.OrderDetails).Include(c => c.Category).SingleOrDefault(predicate);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return product;
        }


        public IEnumerable<Product> FindAll(Expression<Func<Product,bool>> predicate)
        {
            IEnumerable<Product> products = new List<Product>();
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    products = salesDB.Products.Include(o => o.OrderDetails).Include(c => c.Category).Where(predicate).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return products;
        }

        public void AddProduct(Product product)
        {
            try
            {
                var productExist = FindOne(p => p.ProductId == product.ProductId);
                if(productExist == null)
                {
                    using (var salesDB = new As1storeContext())
                    {
                        salesDB.Products.Add(product);
                        salesDB.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("Product already exists");
                }
               

               
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Product> GetProducts()
        {
               IEnumerable<Product> products = new List<Product>();
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    products = salesDB.Products.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return products;
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                Product p = FindOne(item => item.ProductId.Equals(product.ProductId));
                if(p != null)
                {
                    using (var salesDB = new As1storeContext())
                    {
                        salesDB.Products.Update(product);
                        salesDB.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("Product not found");
                }
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public void DeleteProduct(Product product)
        {
            try
            {
                Product p = FindOne(item => item.ProductId.Equals(product.ProductId));
                if (p != null)
                {
                    using (var salesDB = new As1storeContext())
                    {
                        salesDB.Products.Remove(product);
                        salesDB.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("Product not found");
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        public IEnumerable<Product> GetProductByOrderDayNow()
        {
            var orderDay = DateTime.Now.Date;
            IEnumerable<Product> products = new List<Product>();
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    products = salesDB.Products.Where(p => p.OrderDetails.OrderByDescending(m => m.Quantity).Any(od => od.Order.OrderDate == orderDay)).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return products;
        }


    }
}
