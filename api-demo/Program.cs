using api_demo;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DemoContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/customers", async (DemoContext context) =>
{
    var customers = await context.Customers.ToArrayAsync();
    return Results.Ok(customers);
});

app.MapGet(
    "/customer/{id}",
    async (DemoContext context, int id) =>
    {
        var customer = await context.Customers.FindAsync(id);
        if (customer is null)
            return Results.NotFound();
        return Results.Ok(customer);
    }
);

app.MapPost(
    "/customer/new",
    async (DemoContext context, Customer customer) =>
    {
        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();
        return Results.Created($"/customer/{customer.CustomerId}", customer);
    }
);

app.MapGet("/services", async (DemoContext context) =>
{
    var services = await context.Services.ToArrayAsync();
    return Results.Ok(services);
});

app.MapGet("/service/{id}", async (DemoContext context, int id) =>
{
    var service = await context.Services.FindAsync(id);
    if (service is null)
        return Results.NotFound();
    return Results.Ok(service);
});

app.MapPost("/service/new", async (DemoContext context, Service service) =>
{
    await context.Services.AddAsync(service);
    await context.SaveChangesAsync();
    return Results.Created($"/service/{service.ServiceId}", service);
});

app.MapGet("/orders", async (DemoContext context) =>
{
    var orders = await context.CustomerOrders
    .Include(s => s.CustomerServices)
    .ToArrayAsync();
    return Results.Ok(orders);
});

app.MapGet("/order/{id}", async (DemoContext context, int id) =>
{
    var order = await context.CustomerOrders
    .Include(s => s.CustomerServices)
    .FirstOrDefaultAsync(o => o.Id == id);
    if (order is null)
        return Results.NotFound();
    return Results.Ok(order);
});

app.MapPost("/order/new", async (DemoContext context, CustomerOrder order) =>
{
    await context.CustomerOrders.AddAsync(order);
    await context.SaveChangesAsync();
    return Results.Created($"/order/{order.Id}", order);
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
