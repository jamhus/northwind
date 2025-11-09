using System.ComponentModel.DataAnnotations;

namespace Northwind.Models;

public class DashboardConfig
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Key { get; set; } = "default";
    // t.ex. "admin-dashboard", "supplier-dashboard", "northwind-dashboard"

    public int? CompanyId { get; set; }  // om du vill koppla per företag
    public int? SupplierId { get; set; } // eller per leverantör

    [Required]
    public string ConfigJson { get; set; } = "{}";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
