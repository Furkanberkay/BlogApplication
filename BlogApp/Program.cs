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
builder.Services.AddScoped<ITagRepository, EfTagRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();



var app = builder.Build();

app.UseStaticFiles();

SeedData.TestVerileriniDoldur(app);

app.MapControllerRoute(
    name: "post_detail",
    pattern: "/posts/details/{url}",
    defaults: new { controller = "Posts", action = "Detail" }
);

app.MapControllerRoute(
    name:"post_by_tag",
    pattern:"/posts/tag/{tag}",
    defaults:new {controller="Posts", action = "Index"}
);


app.MapControllerRoute(
    name:"default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.Run();


