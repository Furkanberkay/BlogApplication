using System.Security.Claims;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;

        public PostsController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> Index(string tag)
        {
            var claims = User.Claims;
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
        public JsonResult AddComment(int postId, string Text)
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
                publishedOn = entity.PublisedOn,
                avatarUrl = Url.Content("~/img/" + (avatar ?? "bilgisayar-3.png"))
            });
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Create(PostCreateViewModel postCreateViewModel)
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (ModelState.IsValid)
            {
                var post = new Post
                {

                    Title = postCreateViewModel.Title,
                    Description = postCreateViewModel.Description,
                    Url = postCreateViewModel.Url,
                    Content = postCreateViewModel.Content,
                    Image = "bilgisayar-3.png",
                    PublishOn = DateTime.Now,
                    UserId = id,
                    IsActive = false
                };

                _postRepository.CreatePost(post);
                return RedirectToAction("Index");
            }
            return View(postCreateViewModel);
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var posts = _postRepository.Posts;

            if (string.IsNullOrEmpty(role))
            {
                posts = posts.Where(x => x.UserId == userId);
            }
            return View(await posts.ToListAsync());
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = _postRepository.Posts.FirstOrDefault(i => i.PostId == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(new PostEditViewModel
            {
                PostId = id.Value,
                Title = post.Title,
                Content = post.Content,
                Description = post.Description,
                Url = post.Url,
                IsActive = post.IsActive
            });

        }

        // [Authorize]
        // [HttpPost]
        // public IActionResult Edit(PostEditViewModel postEditViewModel)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var editEntity = new Post
        //         {
        //             PostId = postEditViewModel.PostId,
        //             Title = postEditViewModel.Title,
        //             Description = postEditViewModel.Description,
        //             Content = postEditViewModel.Content,
        //             Url = postEditViewModelç
        //         };
        //     }
        //     else
        //     {
        //         return View(postEditViewModel);
        //     }
            
        // }
    }
}