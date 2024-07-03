using ChefApp.Models;
using ChefApp.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmileChef;
using SmileChef.ML;
using SmileChef.Repository;
using SmileChef.Services;
using SmileChef.ViewModels;

var builder = WebApplication.CreateBuilder(args);

#region  Configuring Services
// Add services to the container.
builder.Services.Configure<SqlSettings>(builder.Configuration.GetRequiredSection("SqlSettings"));
builder.Services.AddSingleton<RecipeSmartModel>();
//builder.Services.AddSingleton<ISqlSettings>(sp => sp.GetRequiredService<IOptions<SqlSettings>>().Value);
builder.Services.AddSingleton<ISqlSettings>(sp => sp.GetRequiredService<IOptions<SqlSettings>>().Value);
builder.Services.AddDbContext<ChefAppContext>();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<AutoMapperProfile>();
}); // Add AutoMapper

var mvcBuilder = builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
//mvcBuilder.AddNewtonsoftJson(options =>
//{
//    // Configure Newtonsoft.Json settings here if necessary
//    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
//    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
//});  // Add this line to configure Newtonsoft.Json
//builder.Services.AddScoped(typeof(IRepository<>), implementationType: typeof(GenericRepository<>)); // remove this if you want to mark GenericRepository<T> as abstract
// Register the specific repository for Chef
builder.Services.AddScoped<IChefRepository, ChefRepository>();
builder.Services.AddScoped<IRepository<Recipe>, RecipeRepository>();
builder.Services.AddScoped<IRepository<Instruction>, InstructionRepository>();
builder.Services.AddScoped<IRepository<Subscription>, SubscriptionRepository>();
#endregion

var app = builder.Build();

// Apply migrations (useful for production)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ChefAppContext>();
    try
    {
        dbContext.Database.Migrate(); // Apply any pending migrations
    }
    catch (Exception ex)
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
app.UseSession();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chef}/{action=Index}/{id?}"
    );

app.Run();

// Explicitly declare the partial Program class
public partial class Program { }