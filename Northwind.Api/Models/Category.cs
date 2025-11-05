using System.ComponentModel.DataAnnotations;

namespace Northwind.Models;

public partial class Category
{
    public int CategoryId { get; set; }
    [MaxLength(50)]
    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? Picture { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
