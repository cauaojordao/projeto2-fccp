using Npgsql;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

#region Setup Configuration

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT");

var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";
var redisConnectionString = $"{redisHost}:{redisPort}";

#endregion

#region Initialize Services

await InitPostgreSql(connectionString);

ConnectionMultiplexer? redis = null;
try
{
    redis = await ConnectionMultiplexer.ConnectAsync(redisConnectionString);
}
catch (Exception ex)
{
    Console.WriteLine($"Aviso: Não foi possível conectar ao Redis: {ex.Message}");
}

#endregion

app.MapPost("/products", async (Product product) =>
{
    using var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync();
    
    var command = new NpgsqlCommand(
        "INSERT INTO products (name, price, created_at) VALUES ($1, $2, $3) RETURNING id",
        connection);
    command.Parameters.AddWithValue(product.Name);
    command.Parameters.AddWithValue(product.Price);
    command.Parameters.AddWithValue(DateTime.UtcNow);
    
    var id = await command.ExecuteScalarAsync();
    
    if (redis != null)
    {
        var db = redis.GetDatabase();
        await db.KeyDeleteAsync("products:all");
    }
    
    return Results.Created($"/products/{id}", new { id, product });
});

app.MapGet("/products", async () =>
{
    if (redis != null)
    {
        var db = redis.GetDatabase();
        var cached = await db.StringGetAsync("products:all");
        if (cached.HasValue)
        {
            Console.WriteLine("Retornando dados do cache Redis");
            return Results.Ok(System.Text.Json.JsonSerializer.Deserialize<List<Product>>(cached!));
        }
    }
    
    using var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync();
    
    var command = new NpgsqlCommand("SELECT id, name, price, created_at FROM products ORDER BY created_at DESC", connection);
    var products = new List<Product>();
    
    using var reader = await command.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        products.Add(new Product
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Price = reader.GetDecimal(2),
            CreatedAt = reader.GetDateTime(3)
        });
    }
    
    if (redis != null)
    {
        var db = redis.GetDatabase();
        var json = System.Text.Json.JsonSerializer.Serialize(products);
        await db.StringSetAsync("products:all", json, TimeSpan.FromMinutes(5));
        Console.WriteLine("Dados armazenados no cache Redis");
    }
    
    return Results.Ok(products);
});

app.Run("http://0.0.0.0:8080");

async Task InitPostgreSql(string connString)
{
    try
    {
        using var connection = new NpgsqlConnection(connString);
        await connection.OpenAsync();
        
        var command = new NpgsqlCommand(@"
            CREATE TABLE IF NOT EXISTS products (
                id SERIAL PRIMARY KEY,
                name VARCHAR(255) NOT NULL,
                price DECIMAL(10,2) NOT NULL,
                created_at TIMESTAMP NOT NULL
            )", connection);
        await command.ExecuteNonQueryAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao inicializar banco: {ex.Message}");
    }
}


record Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}

