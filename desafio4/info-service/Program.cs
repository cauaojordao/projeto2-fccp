using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var usersServiceUrl = Environment.GetEnvironmentVariable("USERS_SERVICE_URL");
var httpClient = new HttpClient();

app.MapGet("/info", async () =>
{
    try
    {
        var users = await httpClient.GetFromJsonAsync<List<User>>($"{usersServiceUrl}/users");
        
        if (users == null || !users.Any())
        {
            return Results.Ok(new { message = "Nenhum usuário encontrado", users = new List<object>() });
        }
        
        var info = users.Select(u => new
        {
            u.Id,
            u.Name,
            u.Email,
            ActiveSince = u.ActiveSince,
            DaysActive = (DateTime.UtcNow - u.ActiveSince).Days,
            Status = "Ativo"
        }).ToList();
        
        return Results.Ok(new
        {
            message = $"Encontrados {info.Count} usuários",
            users = info
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: $"Erro ao comunicar com Users Service: {ex.Message}",
            statusCode: 503
        );
    }
});

app.MapGet("/info/{id}", async (int id) =>
{
    try
    {
        var user = await httpClient.GetFromJsonAsync<User>($"{usersServiceUrl}/users/{id}");
        
        if (user == null)
        {
            return Results.NotFound(new { message = "Usuário não encontrado" });
        }
        
        var info = new
        {
            user.Id,
            user.Name,
            user.Email,
            ActiveSince = user.ActiveSince,
            DaysActive = (DateTime.UtcNow - user.ActiveSince).Days,
            Status = "Ativo",
            Message = $"Usuário {user.Name} ativo desde {user.ActiveSince:dd/MM/yyyy} ({((DateTime.UtcNow - user.ActiveSince).Days)} dias)"
        };
        
        return Results.Ok(info);
    }
    catch (HttpRequestException ex)
    {
        return Results.Problem(
            detail: $"Erro ao comunicar com o serviço: {ex.Message}",
            statusCode: 503
        );
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: $"Erro: {ex.Message}",
            statusCode: 500
        );
    }
});

app.Run("http://0.0.0.0:8081");

record User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime ActiveSince { get; set; }
}

