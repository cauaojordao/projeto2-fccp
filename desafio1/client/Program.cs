using System.Net.Http.Json;

var serverUrl = Environment.GetEnvironmentVariable("SERVER_URL") ?? "http://desafio1-server:8080";
var intervalStr = Environment.GetEnvironmentVariable("INTERVAL") ?? "5";
var interval = int.Parse(intervalStr);

Console.WriteLine($"Cliente iniciado");
Console.WriteLine($"Server URL: {serverUrl}");
Console.WriteLine($"Intervalo: {interval} segundos");
Console.WriteLine("");

var httpClient = new HttpClient();
httpClient.Timeout = TimeSpan.FromSeconds(10);

int requestCount = 0;

while (true)
{
    try
    {
        requestCount++;
        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        Console.WriteLine($"[{timestamp}] Requisição #{requestCount}...");

        var response = await httpClient.GetAsync(serverUrl);
        var content = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"  Status: {response.StatusCode}");
        Console.WriteLine($"  Resposta: {content}");
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    await Task.Delay(TimeSpan.FromSeconds(interval));
}
