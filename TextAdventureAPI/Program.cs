using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// JWT config
var jwtSecret = builder.Configuration["JwtSettings:SecretKey"]
                ?? throw new Exception("JWT secret missing!");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

var users = new List<User>();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = key
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


// ---------------------------
// REGISTER
// ---------------------------
app.MapPost("/api/auth/register", ([FromBody] RegisterRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
        return Results.BadRequest("Invalid input.");

    if (users.Any(u => u.Username == req.Username))
        return Results.BadRequest("Username already exists.");

    var hash = ComputeSha256(req.Password);

    users.Add(new User
    {
        Username = req.Username,
        PasswordHash = hash,
        Role = req.Role ?? "Player",
        FailedLogins = 0,
        IsLocked = false
    });

    return Results.Ok("Registered.");
});


// ---------------------------
// LOGIN
// ---------------------------
app.MapPost("/api/auth/login", ([FromBody] LoginRequest req) =>
{
    var user = users.FirstOrDefault(u => u.Username == req.Username);

    if (user == null)
        return Results.BadRequest("User not found.");

    if (user.IsLocked)
        return Results.BadRequest("Account locked.");

    var hash = ComputeSha256(req.Password);

    if (hash != user.PasswordHash)
    {
        user.FailedLogins++;

        if (user.FailedLogins >= 3)
            user.IsLocked = true;

        return Results.BadRequest("Invalid password.");
    }

    user.FailedLogins = 0;

    var token = GenerateJwt(user, key, builder);

    return Results.Ok(new { token });
});


// ---------------------------
// /me endpoint
// ---------------------------
app.MapGet("/api/auth/me", (HttpContext ctx) =>
{
    var user = GetUserFromClaims(ctx);

    if (user == null)
        return Results.Unauthorized();

    return Results.Ok(new { user.Username, user.Role });
}).RequireAuthorization();


// ---------------------------
// KEYSHARE (admin or room 1)
// ---------------------------
app.MapGet("/api/keys/keyshare/{roomId}", (string roomId, HttpContext ctx) =>
{
    var user = GetUserFromClaims(ctx);
    if (user == null)
        return Results.Unauthorized();

    if (user.Role != "Admin" && roomId != "1")
        return Results.BadRequest("You are not allowed to access this keyshare.");

    var keyshare = "secret-share-" + roomId;

    return Results.Ok(keyshare);
}).RequireAuthorization();



app.Run();


// ======================================================
// HELPERS
// ======================================================

static string ComputeSha256(string input)
{
    using var sha = SHA256.Create();
    var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
    return BitConverter.ToString(hash).Replace("-", "").ToLower();
}

static string GenerateJwt(User user, SymmetricSecurityKey key, WebApplicationBuilder builder)
{
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim("username", user.Username),
        new Claim("role", user.Role)
    };

    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        issuer: builder.Configuration["JwtSettings:Issuer"],
        audience: builder.Configuration["JwtSettings:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: creds
    );

    return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
}

static User? GetUserFromClaims(HttpContext ctx)
{
    var username = ctx.User.FindFirst("username")?.Value;
    var role = ctx.User.FindFirst("role")?.Value;

    if (username == null || role == null)
        return null;

    return new User
    {
        Username = username,
        Role = role
    };
}


// ======================================================
// MODELS
// ======================================================

record RegisterRequest(string Username, string Password, string? Role);
record LoginRequest(string Username, string Password);

class User
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public int FailedLogins { get; set; }
    public bool IsLocked { get; set; }
}
