using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    public class ApiClient
    {
        private readonly HttpClient _client;
        private string _jwt = "";
        private readonly string _jwtStorePath;

        public ApiClient(string baseAddress = "http://localhost:5112")
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseAddress);

            _jwtStorePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "textadventure_jwt.dat"
            );

            try
            {
                var existing = LoadJwtFromDisk();
                if (!string.IsNullOrEmpty(existing))
                {
                    _jwt = existing;
                    _client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _jwt);
                }
            }
            catch { }
        }

        public async Task<(bool success, string? error)> Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    return (false, "Gebruikersnaam en wachtwoord mogen niet leeg zijn.");

                var payload = new { username = username.Trim(), password = password };
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                var res = await _client.PostAsync("/api/auth/login", content);

                if (!res.IsSuccessStatusCode)
                {
                    var body = await res.Content.ReadAsStringAsync();
                    return (false, $"Login mislukt: {res.StatusCode} - {body}");
                }

                using var stream = await res.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);

                if (!doc.RootElement.TryGetProperty("token", out var tok))
                    return (false, "Server stuurde geen token terug.");

                _jwt = tok.GetString() ?? "";
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _jwt);

                try { SaveJwtToDisk(_jwt); } catch { }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Onverwachte fout: " + ex.Message);
            }
        }

        public async Task<(string? keyshare, string? error)> GetKeyshare(string roomId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roomId))
                    return (null, "Ongeldig kamer-ID.");

                var res = await _client.GetAsync($"/api/keys/keyshare/{Uri.EscapeDataString(roomId)}");

                if (!res.IsSuccessStatusCode)
                    return (null, $"Keyshare ophalen mislukt: {res.StatusCode}");

                var result = await res.Content.ReadAsStringAsync();
                return (result, null);
            }
            catch (Exception ex)
            {
                return (null, "Fout bij keyshare ophalen: " + ex.Message);
            }
        }

        public async Task<HttpResponseMessage> GetAuthorized(string url)
        {
            EnsureAuthorizationHeader();
            return await _client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAuthorized(string url, HttpContent content)
        {
            EnsureAuthorizationHeader();
            return await _client.PostAsync(url, content);
        }

        private void EnsureAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(_jwt))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _jwt);
            }
        }

        private void SaveJwtToDisk(string jwt)
        {
            var data = Encoding.UTF8.GetBytes(jwt);

            try
            {
                var encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
                File.WriteAllBytes(_jwtStorePath, encrypted);
            }
            catch
            {
                File.WriteAllText(_jwtStorePath, Convert.ToBase64String(data));
            }
        }

        private string? LoadJwtFromDisk()
        {
            if (!File.Exists(_jwtStorePath))
                return null;

            try
            {
                var raw = File.ReadAllBytes(_jwtStorePath);
                var decrypted = ProtectedData.Unprotect(raw, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decrypted);
            }
            catch
            {
                try
                {
                    var base64 = File.ReadAllText(_jwtStorePath);
                    return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
