using ChefApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmileChef.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<SqlSettings>(builder.Configuration.GetRequiredSection("SqlSettings"));
//builder.Services.AddSingleton<ISqlSettings>(sp => sp.GetRequiredService<IOptions<SqlSettings>>().Value);
builder.Services.AddSingleton<ISqlSettings>(sp => sp.GetRequiredService<IOptions<SqlSettings>>().Value);
builder.Services.AddDbContext<ChefAppContext>();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<AutoMapperProfile>();
}); // Add AutoMapper

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Apply migrations (useful for production)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ChefAppContext>();
    try
    {
        dbContext.Database.Migrate(); // Apply any pending migrations
    } catch (Exception ex)
    {
        // Log the error (add logging mechanism)
        Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Explicitly declare the partial Program class
public partial class Program { }