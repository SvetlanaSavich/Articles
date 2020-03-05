using System;

namespace Articles.Services.CommentManagement
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public int UserId { get; set; }

        public int ArticleId { get; set; }
    }
}
