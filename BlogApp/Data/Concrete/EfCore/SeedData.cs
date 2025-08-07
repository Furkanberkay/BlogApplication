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
                    Username = "admin",
                    Email = "berkay@example.com",
                    CreatedOn = DateTime.Now
                };

                var user2 = new User
                {
                    Username = "user1",
                    Email = "user1@example.com",
                    CreatedOn = DateTime.Now
                };

                context.Users.AddRange(user1, user2);
                context.SaveChanges();

                // 2. Etiketler
                var tag1 = new Tag { Text = "Technology" };
                var tag2 = new Tag { Text = "Health" };

                context.Tags.AddRange(tag1, tag2);
                context.SaveChanges();

                // 3. Postlar
                var post1 = new Post
                {
                    Title = "First Post",
                    Content = "This is the content of the first post.",
                    PublishOn = DateTime.Now,
                    IsActive = true,
                    User = user1,
                    Image = "1.jpg",
                    Tags = new List<Tag> { tag1 }
                };

                var post2 = new Post
                {
                    Title = "Second Post",
                    Content = "This is the content of the second post.",
                    PublishOn = DateTime.Now,
                    IsActive = true,
                    User = user2,
                    Image = "2.jpg",
                    Tags = new List<Tag> { tag2 }
                };

                context.Posts.AddRange(post1, post2);
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
