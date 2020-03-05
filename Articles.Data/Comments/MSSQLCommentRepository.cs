using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Articles.Data.Comments
{
    public class MSSQLCommentRepository : ICommentRepository
    {
        private ArticlesDbContext context;

        public MSSQLCommentRepository(ArticlesDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Comment>> GetCommentsAsync()
        {
            return await this.context.Comments.ToListAsync();
        }

        public async Task<Comment> GetCommentAsync(int commentId)
        {
            return await context.Comments.FindAsync(commentId);
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            context.Comments.Add(comment);

            await context.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            context.Entry(comment).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return comment;
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await context.Comments.FindAsync(commentId);

            context.Comments.Remove(comment);

            await context.SaveChangesAsync();
        }
    }
}