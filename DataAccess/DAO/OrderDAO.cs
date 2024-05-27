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
    public class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();

        private OrderDAO() { }
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }

            }
        }

        public Order FindOne(Expression<Func<Order, bool>> predicate)
        {
            Order order = null;
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    order = salesDB.Orders.Include(o => o.Member).SingleOrDefault(predicate);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return order;
        }

        public IEnumerable<Order> FindAll(Expression<Func<Order, bool>> predicate)
        {
            IEnumerable<Order> orders = new List<Order>();
            using (var salesDB = new As1storeContext())
            {
                orders = salesDB.Orders.Include(o => o.Member).Where(predicate).ToList();
            }
            return orders;
        }


        public void AddOrder(Order order)
        {
            try
            {
                Order o = FindOne(x => x.OrderId == order.OrderId);
                if(o == null)
                {
                    using (var salesDB = new As1storeContext())
                    {
                        foreach(OrderDetail orderDetail in order.OrderDetails)
                        {
                            Product product = ProductDAO.Instance.FindOne(p => p.ProductId == orderDetail.ProductId);
                            product.UnitsInStock -= orderDetail.Quantity;
                            ProductDAO.Instance.UpdateProduct(product);
                        }

                        salesDB.Orders.Add(order);
                        salesDB.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("Order already exists");
                }
               
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Order> GetListOrder()
        {
            IEnumerable<Order> orders = new List<Order>();
            using (var salesDB = new As1storeContext())
            {
                orders = salesDB.Orders.Include(m => m.Member).ToList();
            }
            return orders;
        }

        public void UpdateOrder(Order order)
        {
            try
            {
                Order o = FindOne(x => x.OrderId == order.OrderId);
                if (o != null)
                {
                    using (var salesDB = new As1storeContext())
                    {
                        salesDB.Orders.Update(order);
                        salesDB.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("Order not found");
                }
               
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteOrder(Order order)
        {
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    salesDB.Orders.Remove(order);
                    salesDB.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
