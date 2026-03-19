using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using QuickTable.API.Extensions;
using QuickTable.Service.Models;
using QuickTable.Service.Repositoies.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174",
                 "http://localhost:5175",
                "https://quicktable.store",
                "https://www.quicktable.store"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddDbContext<QuickTableContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quick Table API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseCors("AllowAll");

var uploadsPath = Environment.GetEnvironmentVariable("UPLOAD_PATH")
    ?? Path.Combine(builder.Environment.WebRootPath ??
       builder.Environment.ContentRootPath, "uploads");

Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
