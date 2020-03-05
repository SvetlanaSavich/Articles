using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Articles.Data.Comments;
using Articles.Data.Users;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Articles.Data.Articles
{
    public class Article
    {
        [BsonId]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [BsonIgnore]
        public ArticleCategory Category { get; set; }

        [Required]
        public int UserId { get; set; }

        [BsonIgnore]
        public User User { get; set; }

        [BsonIgnore]
        public List<Comment> Comments { get; set; }
    }
}