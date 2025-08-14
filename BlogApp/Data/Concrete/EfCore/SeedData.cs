using System;
using System.Collections.Generic;
using System.Linq;
using BlogApp.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BlogContext>();

            // Eğer bekleyen migration varsa uygula
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            // Eğer veriler daha önce eklenmemişse
            if (!context.Users.Any())
            {
                // 1. Kullanıcılar
                var user1 = new User
                {
                    Username = "furkanberkay",
                    Email = "berkay@example.com",
                    Image = "bilgisayar-2.png",
                    Name = "Berkay",
                    Password = "berkay123",
                    CreatedOn = DateTime.Now
                };

                var user2 = new User
                {
                    Username = "dila",
                    Email = "user1@example.com",
                    Image = "bilgisayar.png",
                    Name = "Dila",
                    Password = "dila123",
                    CreatedOn = DateTime.Now
                };

                context.Users.AddRange(user1, user2);
                context.SaveChanges();

                // 2. Etiketler
                var tag1 = new Tag { Text = "Web Programlama", Url = "web-programlama",Color = TagColors.success };
                var tag2 = new Tag { Text = "Backend", Url = "backend", Color = TagColors.warning};
                var tag3 = new Tag { Text = "Frontend", Url = "frontend",Color=TagColors.primary };
                var tag4 = new Tag { Text = "Mobil Programlama", Url = "mobil-programlama",Color=TagColors.secondary };

                context.Tags.AddRange(tag1, tag2, tag3, tag4);
                context.SaveChanges();

                // 3. Postlar
                var post1 = new Post
                {
                    Title = "ASP .NET",
                    Content = "This is the content of the first post.",
                    PublishOn = DateTime.Now,
                    IsActive = true,
                    Url = "asp-core",
                    User = user1,
                    Image = "1.jpg",
                    Tags = new List<Tag> { tag1 },
                    Comments = new List<Comment>
                    {
                        new Comment {Text = "çok sağlam kurs", PublisedOn = new DateTime(),UserId = 1},
                        new Comment {Text ="efsane", PublisedOn = new DateTime(),UserId =2}
                    }
                };

                var post2 = new Post
                {
                    Title = "Flutter Post",
                    Content = "This is the content of the second post.",
                    PublishOn = DateTime.Now,
                    IsActive = true,
                    Url = "flutter",
                    User = user2,
                    Image = "2.jpg",
                    Tags = new List<Tag> { tag2 }
                };

                var post3 = new Post
                {
                    Title = "React Basics",
                    Content = "This post covers the basics of React.",
                    PublishOn = DateTime.Now.AddMinutes(5),
                    IsActive = true,
                    Url = "react-basics",
                    User = user1,
                    Image = "3.jpg",
                    Tags = new List<Tag> { tag3 }
                };

                var post4 = new Post
                {
                    Title = "Node.js Introduction",
                    Content = "Learn the fundamentals of Node.js.",
                    PublishOn = DateTime.Now.AddMinutes(10),
                    IsActive = true,
                    Url = "nodejs-intro",
                    User = user2,
                    Image = "4.jpg",
                    Tags = new List<Tag> { tag2 }
                };

                var post5 = new Post
                {
                    Title = "Mobile App Development",
                    Content = "Learn how to build mobile apps with Flutter.",
                    PublishOn = DateTime.Now.AddMinutes(15),
                    IsActive = true,
                    Url = "mobile-development",
                    User = user1,
                    Image = "5.jpg",
                    Tags = new List<Tag> { tag4 }
                };

                var post6 = new Post
                {
                    Title = "Advanced JavaScript",
                    Content = "Explore advanced topics in JavaScript.",
                    PublishOn = DateTime.Now.AddMinutes(20),
                    IsActive = true,
                    Url = "advanced-js",
                    User = user2,
                    Image = "6.jpg",
                    Tags = new List<Tag> { tag3 }
                };

                context.Posts.AddRange(post1, post2, post3, post4, post5, post6);
                context.SaveChanges();

                // 4. Yorumlar
                var comment1 = new Comment
                {
                    Text = "This is a comment on the first post.",
                    Post = post1,
                    User = user1,
                    PublisedOn = DateTime.Now
                };

                var comment2 = new Comment
                {
                    Text = "This is another comment on the first post.",
                    Post = post1,
                    User = user2,
                    PublisedOn = DateTime.Now
                };

                context.Comments.AddRange(comment1, comment2);
                context.SaveChanges();
            }
        }
    }
}
