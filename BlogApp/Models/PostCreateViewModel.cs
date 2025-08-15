using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class PostCreateViewModel
    {
        [Required]
        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "İçerik")]
        public string? Content { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "url")]
        public string? Url { get; set; }
    }
}