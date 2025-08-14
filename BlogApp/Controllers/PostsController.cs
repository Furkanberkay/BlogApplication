using System.Security.Claims;
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
        private  IPostRepository _postRepository;
        private ICommentRepository _commentRepository;

        public PostsController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> Index(string tag)
        {
            var claiims = User.Claims;
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
        [HttpPost]
        public  JsonResult AddComment(int postId,string Text)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);

            var entity = new Comment
            {
                Text = Text,
                PostId = postId,
                PublisedOn = DateTime.Now,
                UserId = int.Parse(userId ?? ""),


                
            };
            _commentRepository.CreateComment(entity);
            return Json(new
            {
                username,
                text = Text,
                publishedOn=entity.PublisedOn,
                avatarUrl = Url.Content("~/img/" + (avatar ?? "bilgisayar-3.png"))
            });
        }
    }
}