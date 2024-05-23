using BusinessObject.Entity;
using BusinessObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderDetailRepository
    {
        void AddOrderDetail(OrderDetail orderDetail);
        void UpdateOrderDetail(OrderDetail orderDetail);
        void RemoveOrderDetail(OrderDetail orderDetail);
        IEnumerable<OrderDetail> GetAllOrderDetail();
        IEnumerable<OrderDetail> FindByOrderId(int orderId);
        IEnumerable<OrderDetail> FindByProductId(int productId);
        IEnumerable<OrderDetail> FindAllByUser(int userID);
        OrderDetail FindByOrderIdAndProductId(int orderId, int productId);
        IEnumerable<OrderDetail> ListDettailOrderIdAndProductId(int orderId, int productId);
        IEnumerable<ReportSaleObject> ReportSales();
    }
}
