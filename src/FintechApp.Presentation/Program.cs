using System.Reflection;
using System.Security.Claims;
using System.Text;
using FintechApp.Presentation.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using dotenv.net;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

var configuration = builder.Configuration;

var connectionString =
    $"Host={Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost"};" +
    $"Port={Environment.GetEnvironmentVariable("DB_PORT") ?? "5432"};" +
    $"Database={Environment.GetEnvironmentVariable("DB_NAME") ?? "fintech"};" +
    $"Username={Environment.GetEnvironmentVariable("DB_USER") ?? "postgres"};" +
    $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "sa"};" +
    $"Ssl Mode={Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Disable"};" +
    $"Trust Server Certificate={Environment.GetEnvironmentVariable("DB_TRUST_SERVER_CERT") ?? "true"}";


// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Services
builder.Services.AddApplicationServices(configuration);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Authentication & Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") 
                 ?? configuration["Jwt:Key"];
    var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") 
                    ?? configuration["Jwt:Issuer"];
    var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
                      ?? configuration["Jwt:Audience"];

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        RoleClaimType = ClaimTypes.Role
    };

    // Map "sub" claim vào NameIdentifier để đọc userId dễ dàng
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            if (context.Principal?.FindFirst("sub") is Claim subClaim)
            {
                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                claimsIdentity?.AddClaim(new Claim(ClaimTypes.NameIdentifier, subClaim.Value));
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiPermission", policy =>
        policy.Requirements.Add(new DenyAnonymousAuthorizationRequirement()));
});

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Fintech API V1",
        Version = "v1",
        Description = "API documentation for Fintech application"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập JWT token theo format: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (app.Environment.IsDevelopment())
    {
      //  db.Database.Migrate();
    }
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fintech API V1");
    c.RoutePrefix = string.Empty;
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
