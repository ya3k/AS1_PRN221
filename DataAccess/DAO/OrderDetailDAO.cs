using BusinessObject.Entity;
using BusinessObject.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    internal class OrderDetailDAO
    {
        private static OrderDetailDAO instance = null;
        private static readonly object instanceLock = new object();

        private OrderDetailDAO() { }
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }

            }
        }   

        public OrderDetail FindOne(Expression<Func<OrderDetail, bool>> pridecate)
        {
            OrderDetail orderDetail = null;
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    orderDetail = salesDB.OrderDetails.Include(m => m.Order).Include(m => m.Product).FirstOrDefault(pridecate);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return orderDetail;
        }

        public IEnumerable<OrderDetail> FindAll(Expression<Func<OrderDetail, bool>> pridecate)
        {
            IEnumerable<OrderDetail> orderDetails = new List<OrderDetail>();
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    orderDetails = salesDB.OrderDetails.Where(pridecate).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return orderDetails;
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                OrderDetail od = FindOne(m => m.OrderId == orderDetail.OrderId && m.ProductId == orderDetail.ProductId);
                if (od == null)
                {
                    using (var salesDB = new As1storeContext())
                    {
                        salesDB.OrderDetails.Add(orderDetail);
                        salesDB.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                OrderDetail od = FindOne(m => m.OrderId == orderDetail.OrderId && m.ProductId == orderDetail.ProductId);
                if (od != null)
                {
                    using (var salesDB = new As1storeContext())
                    {
                        salesDB.OrderDetails.Update(orderDetail);
                        salesDB.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<OrderDetail> GetAllOrderDetail()
        {
            try
            {
                IEnumerable<OrderDetail> orderDetails = new List<OrderDetail>();
                using (var salesDB = new As1storeContext())
                {
                    orderDetails = salesDB.OrderDetails.ToList();
                }
                return orderDetails;
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                
                using(var salesDB = new As1storeContext())
                {
                    salesDB.Remove(orderDetail);
                    salesDB.SaveChanges();
                }
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public IEnumerable<ReportSaleObject> CountProductReport()
        {
            try
            {
                List<ReportSaleObject> saleObjects = new List<ReportSaleObject>();
                using (var salesDB = new As1storeContext())
                {
                    var totalQuantity = salesDB.OrderDetails
                        .Include(p => p.Product)
                        .Include(o => o.Order)
                        .GroupBy(m => m.ProductId)
                        .Select(m => new
                        {
                            ProductId = m.Key,  
                            ProductName = m.Select(m => m.Product.ProductName).FirstOrDefault(),
                            TotalQuantity = m.Sum(m => m.Quantity),
                            Total = m.Sum(m => m.Quantity * m.Product.UnitPrice)
                        })
                        .ToList();

                    foreach (var item in totalQuantity)
                    {
                        ReportSaleObject orderDetail = new ReportSaleObject
                        {
                            ProductId = item.ProductId,
                            Quantity = item.TotalQuantity,
                            ProductName = item.ProductName,
                            Total = item.Total,

                            
                        };
                        saleObjects.Add(orderDetail);
                    }
                }
                return saleObjects;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
