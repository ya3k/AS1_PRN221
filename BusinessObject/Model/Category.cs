using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual IEnumerable<Product>? Product { get; set; }
}
