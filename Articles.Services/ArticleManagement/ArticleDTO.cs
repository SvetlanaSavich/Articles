using System;

namespace Articles.Services.ArticleManagement
{
    public class ArticleDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CategoryId { get; set; }

        public int UserId { get; set; }
    }
}