using Microsoft.AspNetCore.Identity;

namespace Northwind.Models;

public class ApplicationUser : IdentityUser
{
    public int? EmployeeId { get; set; }
    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public Employee? Employee { get; set; }
}
