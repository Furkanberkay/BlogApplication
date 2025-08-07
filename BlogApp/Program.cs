using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BlogContext>(option =>
{
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("sql_conenction");
    option.UseSqlite(connectionString);
});

builder.Services.AddScoped<IPostRepository, EfPostRepository>();

var app = builder.Build();

SeedData.TestVerileriniDoldur(app);

app.MapDefaultControllerRoute();
app.Run();
