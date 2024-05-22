using BusinessObject.Entity;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void AddOrderDetail(OrderDetail orderDetail)
        {
            OrderDetailDAO.Instance.AddOrderDetail(orderDetail);
        }

    
        public IEnumerable<OrderDetail> FindAllByUser(int userID)
        {
            return OrderDetailDAO.Instance.FindAll(orderDetail => orderDetail.Order.Member.MemberId == userID);
        }

        public IEnumerable<OrderDetail> FindByOrderId(int orderId)
        {
            return OrderDetailDAO.Instance.FindAll(orderDetail => orderDetail.OrderId == orderId);
        }

        public IEnumerable<OrderDetail> FindByOrderIdAndProductId(int orderId, int productId)
        {
           return OrderDetailDAO.Instance.FindAll(orderDetail => orderDetail.OrderId == orderId && orderDetail.ProductId == productId);
        }

        public IEnumerable<OrderDetail> FindByProductId(int productId)
        {
            return OrderDetailDAO.Instance.FindAll(orderDetail => orderDetail.ProductId == productId);
        }

        public IEnumerable<OrderDetail> GetAllOrderDetail()
        {
          return OrderDetailDAO.Instance.GetAllOrderDetail();
        }

        public void RemoveOrderDetail(OrderDetail orderDetail)
        {
           OrderDetailDAO.Instance.DeleteOrderDetail(orderDetail);
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            OrderDetailDAO.Instance.UpdateOrderDetail(orderDetail);
        }
    }
}
