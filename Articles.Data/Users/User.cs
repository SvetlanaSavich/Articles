using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using Articles.Data.Articles;
using Articles.Data.Comments;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Articles.Data.Users
{
    public class User
    {
        [BsonId]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [BsonIgnore]
        public List<Article> Articles { get; set; }

        [BsonIgnore]
        public List<Comment> Comments { get; set; }
    }
}