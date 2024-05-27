using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Entity;

public partial class Product
{
    public Product()
    {
        OrderDetails = new HashSet<OrderDetail>();
    }

    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public string ProductName { get; set; }
    public float Weight { get; set; }
    public decimal UnitPrice { get; set; }
    public int UnitsInStock { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    public virtual Category Category { get; set; }

    private int _quantity = 0;
    [NotMapped]
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity != value)
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
