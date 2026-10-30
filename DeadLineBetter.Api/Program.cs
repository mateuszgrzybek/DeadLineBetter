using DeadLineBetter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not configured.");
}

builder.Services.AddDbContext<DeadLineBetterDbContext>(
    options => options.UseNpgsql(connectionString));

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();