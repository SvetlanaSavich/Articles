using System;
using System.ComponentModel.DataAnnotations;

namespace Articles.Services.ArticleManagement
{
    public class UpdateArticleRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Content { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int CategoryId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int UserId { get; set; }
    }
}