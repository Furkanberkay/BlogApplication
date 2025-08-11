using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IActionResult> Index(string tag)
        {
            var post = _postRepository.Posts;
            if (!string.IsNullOrEmpty(tag))
            {
                post = post.Where(x => x.Tags.Any(t => t.Url == tag));
            }
            return View(
                new PostViewModel
                {
                    Posts = await post.ToListAsync()
                }
            );
        }

        public async Task<IActionResult> Detail(string? url)
        {
            return View(await _postRepository.Posts
            .Include(x => x.Tags)
            .Include(y => y.Comments)
            .ThenInclude(u => u.User)
            .FirstOrDefaultAsync(p => p.Url == url));
        }
    }
}