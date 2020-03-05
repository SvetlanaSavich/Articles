using System.ComponentModel.DataAnnotations;

namespace Articles.Services.ArticleManagement
{
    public class UpdateArticleCategoryRequest
    {
        [Required]
        public string CategoryName { get; set; }
    }
}