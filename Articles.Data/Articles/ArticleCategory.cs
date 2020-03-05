using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Articles.Data.Articles
{
    public class ArticleCategory
    {
        [BsonId]
        public int Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [BsonIgnore]
        public List<Article> Articles { get; set; }
    }
}