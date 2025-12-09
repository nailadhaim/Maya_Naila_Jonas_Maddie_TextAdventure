using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class ApiClient
{
    private readonly HttpClient _client;
    private string _jwt = "";

    public ApiClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://localhost:5112");
    }

    public async Task<bool> Login(string username, string password)
    {
        var payload = new
        {
            Username = username,
            Password = password
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var res = await _client.PostAsync("/api/auth/login", content);

        if (!res.IsSuccessStatusCode)
            return false;

        var body = await res.Content.ReadAsStringAsync();
        var data = JsonDocument.Parse(body);

        _jwt = data.RootElement.GetProperty("token").GetString();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);

        return true;
    }

    public async Task<string?> GetKeyshare(string roomId)
    {
        var res = await _client.GetAsync($"/api/keys/keyshare/{roomId}");

        if (!res.IsSuccessStatusCode)
            return null;

        return await res.Content.ReadAsStringAsync();
    }
}