using System;
using System.ComponentModel.DataAnnotations;

namespace Articles.Services.CommentManagement
{
    public class UpdateCommentRequest
    {
        [Required]
        [MaxLength(200)]
        public string Content { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int ArticleId { get; set; }
    }
}