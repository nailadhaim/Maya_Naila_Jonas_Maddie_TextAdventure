using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TextAdventure API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Description = "Put ONLY your JWT token here (no 'Bearer ' prefix)",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var jwtSecret = builder.Configuration["JwtSettings:SecretKey"]
                ?? throw new Exception("JWT secret missing!");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

var users = new List<User>();

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
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TextAdventure API V1");
    c.RoutePrefix = "swagger";
});

if (app.Environment.IsDevelopment())
{
    var url = app.Urls.FirstOrDefault() ?? "https://localhost:7192/swagger/index.html";
    try
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
    catch
    {
       
    }
}

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

app.MapGet("/api/auth/me", (HttpContext ctx) =>
{
    var user = GetUserFromClaims(ctx);
    if (user == null)
        return Results.Unauthorized();

    return Results.Ok(new { user.Username, user.Role });
}).RequireAuthorization();

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
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
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
    var username = ctx.User.Identity?.Name;
    var role = ctx.User.FindFirst(ClaimTypes.Role)?.Value;


    if (username == null || role == null)
        return null;

    return new User
    {
        Username = username,
        Role = role,
        PasswordHash = string.Empty, 
        IsLocked = false
    };
}

record RegisterRequest(string Username, string Password, string? Role);
record LoginRequest(string Username, string Password);

class User
{
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Role { get; set; }
    public int FailedLogins { get; set; }
    public bool IsLocked { get; set; }
}
