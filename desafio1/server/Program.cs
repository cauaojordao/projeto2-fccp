var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet(
    "/",
    () =>
    {
        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        Console.WriteLine($"[{timestamp}] Requisição recebida");
        return Results.Ok(new { message = "Servidor rodando!", timestamp = timestamp });
    }
);

app.Run("http://0.0.0.0:8080");
