using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Articles.Data.Articles;
using Articles.Data.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Articles.Data.Comments
{
    public class Comment
    {
        [BsonId]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [BsonIgnore]
        public User User { get; set; }

        [Required]
        public int ArticleId { get; set; }

        [BsonIgnore]
        public Article Article { get; set; }
    }
}