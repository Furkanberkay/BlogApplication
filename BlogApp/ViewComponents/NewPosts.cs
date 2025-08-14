using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class NewPosts : ViewComponent
    {
        private IPostRepository _postRepository;
        public NewPosts(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await
                _postRepository
                .Posts
                .OrderByDescending(p => p.PublishOn)  //Blog, haber, duyuru gibi listelerde en güncel olanı önde göstermek için OrderByDescending kullanırız.
                .Take(5) //Sıralamadan sonra ilk 5 kayıt alınıyor.
                .ToListAsync()
                );
        }
    }
}