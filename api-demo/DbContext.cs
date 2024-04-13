using Microsoft.EntityFrameworkCore;

namespace api_demo;

public class DemoContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<CustomerOrder> CustomerOrders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlServer(
            $"ConnectionString"
        );
}

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class Service
{
    public int ServiceId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class CustomerOrder
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public List<Service> CustomerServices { get; set; } = [];
    public Service? Service { get; set; }
}
public class CustomerOrderDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public List<Service> CustomerServices { get; set; } = [];
}
