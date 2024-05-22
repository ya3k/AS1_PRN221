using BusinessObject.Entity;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public void Add(Order order)
        {
            OrderDAO.Instance.AddOrder(order);  
        }

        public IEnumerable<Order> FindAllByStartTimeAndEndTime(DateTime start, DateTime end)
        {
            return OrderDAO.Instance.FindAll(order => order.OrderDate >= start && order.OrderDate <= end);
        }

        public IEnumerable<Order> FindByEmail(string email)
        {
           return OrderDAO.Instance.FindAll(order => order.Member.Email == email);
        }

        public Order FindById(int id)
        {
           return OrderDAO.Instance.FindOne(order => order.OrderId == id);
        }

        public IEnumerable<Order> FindByUserID(int memId)
        {
            return OrderDAO.Instance.FindAll(order => order.Member.MemberId == memId);
        }

        public IEnumerable<Order> List()
        {
            return OrderDAO.Instance.GetListOrder();
        }

        public void Remove(Order order)
        {
           OrderDAO.Instance.DeleteOrder(order);
        }

        public void Update(Order order)
        {
           OrderDAO.Instance.UpdateOrder(order);
        }
    }
}
