using System.Reflection;
using FintechApp.Presentation.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fintech API V1", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fintech API V1");
    c.RoutePrefix = string.Empty;
});

app.UseAuthorization();
app.MapControllers();
app.Run();


//using FintechApp.Presentation.Extensions;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddApplicationServices(builder.Configuration);
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        policy => policy.AllowAnyOrigin()
//                        .AllowAnyMethod()
//                        .AllowAnyHeader());
//});
//var app = builder.Build();
//app.UseCors("AllowAll");
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    db.Database.Migrate();
//}

//// Bật Swagger
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fintech API V1");
//    c.RoutePrefix = string.Empty;
//});

//// app.UseHttpsRedirection(); // comment khi chạy Docker
//app.UseAuthorization();
//app.MapControllers();
//app.Run();

