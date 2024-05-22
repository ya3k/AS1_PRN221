using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp
{
    public class Cart
    {
        public static string? Email { get; set; } = null;
        public static List<OrderDetail> carts { get; set; } = null;
        public static int Discount { get; set; } = 0;
        public static int Freight { get; set; } = 30;
    }
}
