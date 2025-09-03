using FintechApp.Presentation.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Add DI for services & repositories
builder.Services.AddApplicationServices(builder.Configuration);
// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // <-- Ph?i c� d�ng n�y

// Add DI c?a project
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();           // <-- d�ng sau AddSwaggerGen
    app.UseSwaggerUI();         // <-- d�ng sau AddSwaggerGen
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
