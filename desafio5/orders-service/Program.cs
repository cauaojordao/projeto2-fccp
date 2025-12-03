var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var orders = new List<Order>
{
    new Order { Id = 1, UserId = 1, Product = "Notebook", Amount = 2500.00m, CreatedAt = new DateTime(2024, 1, 10) },
    new Order { Id = 2, UserId = 1, Product = "Mouse", Amount = 50.00m, CreatedAt = new DateTime(2024, 1, 15) },
    new Order { Id = 3, UserId = 2, Product = "Teclado", Amount = 150.00m, CreatedAt = new DateTime(2024, 2, 5) },
    new Order { Id = 4, UserId = 2, Product = "Monitor", Amount = 800.00m, CreatedAt = new DateTime(2024, 2, 20) },
    new Order { Id = 5, UserId = 1, Product = "Webcam", Amount = 200.00m, CreatedAt = new DateTime(2024, 3, 1) }
};

app.MapGet("/orders", () => 
{
    return Results.Ok(orders);
});

app.MapGet("/orders/{id}", (int id) =>
{
    var order = orders.FirstOrDefault(o => o.Id == id);
    if (order == null)
        return Results.NotFound(new { message = "Pedido não encontrado" });
    
    return Results.Ok(order);
});

app.MapGet("/orders/user/{userId}", (int userId) =>
{
    var userOrders = orders.Where(o => o.UserId == userId).ToList();
    return Results.Ok(userOrders);
});

app.Run("http://0.0.0.0:8080");

record Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Product { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}

