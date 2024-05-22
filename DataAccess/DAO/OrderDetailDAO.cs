using BusinessObject.Entity;
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

        public OrderDetail FindOne(int orderId, int productId)
        {
            OrderDetail orderDetail = null;
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    orderDetail = salesDB.OrderDetails.SingleOrDefault(od => od.OrderId == orderId && od.ProductId == productId);
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
                OrderDetail od = FindOne(orderDetail.OrderId, orderDetail.ProductId);
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
                OrderDetail od = FindOne(orderDetail.OrderId, orderDetail.ProductId);
                if (od != null)
                {
                    using (var salesDB = new As1storeContext())
                    {
                        salesDB.Entry(od).CurrentValues.SetValues(orderDetail);
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
        

    }
}
