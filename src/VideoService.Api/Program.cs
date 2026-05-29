using Microsoft.EntityFrameworkCore;
using VideoService.Api.Security;
using VideoService.Api.OpenApi;
using VideoService.Application.Services;
using VideoService.Domain.Interfaces;
using VideoService.Infrastructure.Contexts;
using VideoService.Infrastructure.Repositories;
using VideoService.Infrastructure.Services;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<VideoDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IVideoRepository, VideoRepository>();

builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

builder.Services.AddScoped<VideoManager>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/docs", options =>
    {
        options.Title = "LMS Video API";
        options.Theme = ScalarTheme.Laserwave;
    });
}

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();