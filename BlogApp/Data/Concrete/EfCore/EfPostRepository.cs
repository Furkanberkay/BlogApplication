using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfPostRepository : IPostRepository
    {
        private readonly BlogContext _context;
        public EfPostRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Posts => _context.Posts;

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }



        public void EditPost(Post post, int[] tagIds)
        {
            var entity = _context.Posts.Include(x => x.Tags).FirstOrDefault(x => x.PostId == post.PostId);

            if (entity != null)
            {
                entity.PostId = post.PostId;
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.IsActive = post.IsActive;
                entity.Content = post.Content;
                entity.Url = post.Url;

                entity.Tags = _context.Tags.Where(q => tagIds.Contains(q.TagId)).ToList();      
                _context.SaveChanges();
            }
        }
    }
}