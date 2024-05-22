using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Entity;

public partial class Order
{

    public Order()
    {
        OrderDetails = new HashSet<OrderDetail>();
    }

    public int OrderId { get; set; }
    public int MemberId { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime OrderDate { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? RequiredDate { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? ShippedDate { get; set; }
    public decimal? Freight { get; set; }

    public virtual Member Member { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }

}
