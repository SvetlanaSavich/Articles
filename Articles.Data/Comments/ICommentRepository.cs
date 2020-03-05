using System.Collections.Generic;
using System.Threading.Tasks;

namespace Articles.Data.Comments
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetCommentsAsync();

        Task<Comment> GetCommentAsync(int commentId);

        Task<Comment> CreateCommentAsync(Comment comment);

        Task<Comment> UpdateCommentAsync(Comment comment);

        Task DeleteCommentAsync(int commentId);
    }
}