using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Articles.Services;
using MongoDB.Driver;

namespace Articles.Data.Comments
{
    public class MongoCommentRepository : ICommentRepository
    {
        private readonly MongoDbContext context;

        public MongoCommentRepository(MongoDbContext context)
        {
            this.context = context;

            if (context.Comments.CountDocuments(_ => true) == 0)
            {
                var comment = new Comment
                {
                    Id = 1,
                    ArticleId = 1,
                    Content = "Default Content",
                    UserId = 1,
                    CreateDate = DateTime.Now
                };

                context.Comments.InsertOne(comment);
            }
        }

        public async Task<List<Comment>> GetCommentsAsync()
        {
            return await context.Comments.Find(_ => true).ToListAsync();
        }

        public async Task<Comment> GetCommentAsync(int commentId)
        {
            return await context.Comments.Find(c => c.Id == commentId).FirstOrDefaultAsync();
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            await CheckThatCommentArticleIdExists(comment.ArticleId);

            await CheckThatCommentUserIdExists(comment.UserId);

            comment.Id = await GenerateCommentId();

            await context.Comments.InsertOneAsync(comment);

            return comment;
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            await context.Comments.ReplaceOneAsync(c => c.Id == comment.Id, comment);

            return comment;
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            await context.Comments.DeleteOneAsync(c => c.Id == commentId);
        }

        private async Task<int> GenerateCommentId()
        {
            var comments = await GetCommentsAsync();

            return comments.Select(c => c.Id).Max() + 1;
        }

        private async Task CheckThatCommentUserIdExists(int userId)
        {
            var user = await context.Users.Find(u => u.Id == userId).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ResourceNotFoundException(("User with given id does not exists."));
            }
        }

        private async Task CheckThatCommentArticleIdExists(int articleId)
        {
            var article = await context.Articles.Find(a => a.Id == articleId).FirstOrDefaultAsync();

            if (article == null)
            {
                throw new ResourceNotFoundException("Article with given id does not exists.");
            }
        }
    }
}