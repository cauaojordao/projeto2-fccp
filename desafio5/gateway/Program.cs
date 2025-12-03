using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var usersServiceUrl = Environment.GetEnvironmentVariable("USERS_SERVICE_URL");
var ordersServiceUrl = Environment.GetEnvironmentVariable("ORDERS_SERVICE_URL");

var httpClient = new HttpClient();


app.MapGet("/users", async () =>
{
    try
    {
        var response = await httpClient.GetAsync($"{usersServiceUrl}/users");
        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<List<User>>();
        return Results.Ok(users);
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: $"Erro ao comunicar com Users Service: {ex.Message}",
            statusCode: 503
        );
    }
});

app.MapGet("/users/{id}", async (int id) =>
{
    try
    {
        var response = await httpClient.GetAsync($"{usersServiceUrl}/users/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return Results.NotFound(new { message = "Usuário não encontrado" });
        
        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<User>();
        return Results.Ok(user);
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: $"Erro ao comunicar com Users Service: {ex.Message}",
            statusCode: 503
        );
    }
});

app.MapGet("/orders", async () =>
{
    try
    {
        var response = await httpClient.GetAsync($"{ordersServiceUrl}/orders");
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<Order>>();
        return Results.Ok(orders);
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: $"Erro ao comunicar com Orders Service: {ex.Message}",
            statusCode: 503
        );
    }
});

app.MapGet("/orders/{id}", async (int id) =>
{
    try
    {
        var response = await httpClient.GetAsync($"{ordersServiceUrl}/orders/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return Results.NotFound(new { message = "Pedido não encontrado" });
        
        response.EnsureSuccessStatusCode();
        var order = await response.Content.ReadFromJsonAsync<Order>();
        return Results.Ok(order);
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: $"Erro ao comunicar com Orders Service: {ex.Message}",
            statusCode: 503
        );
    }
});

app.MapGet("/orders/user/{userId}", async (int userId) =>
{
    try
    {
        var response = await httpClient.GetAsync($"{ordersServiceUrl}/orders/user/{userId}");
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<Order>>();
        return Results.Ok(orders);
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: $"Erro ao comunicar com Orders Service: {ex.Message}",
            statusCode: 503
        );
    }
});

app.Run("http://0.0.0.0:8080");

record User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

record Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Product { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}

