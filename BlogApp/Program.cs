using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogContext>(option =>
{
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("sql_conenction");
    option.UseSqlite(connectionString);
});

var app = builder.Build();

SeedData.TestVerileriniDoldur(app);

app.MapGet("/", () => "Hello World!");

app.Run();
