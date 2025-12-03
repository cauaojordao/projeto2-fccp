var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var users = new List<User>
{
    new User { Id = 1, Name = "Jorge Soares", Email = "jorge.soares@cesar.school", ActiveSince = new DateTime(2023, 1, 15) },
    new User { Id = 2, Name = "Cauã Jordão", Email = "coj@cesar.school", ActiveSince = new DateTime(2023, 3, 20) },
};

app.MapGet("/users", () => 
{
    return Results.Ok(users);
});

app.MapGet("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound(new { message = "Usuário não encontrado" });
    
    return Results.Ok(user);
});

app.Run("http://0.0.0.0:8080");

record User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime ActiveSince { get; set; }
}

