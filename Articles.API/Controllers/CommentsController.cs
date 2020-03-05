using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Articles.Data;
using Articles.Services;
using Articles.Services.CommentManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Articles.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentService commentService;

        public CommentsController(CommentService commentService)
        {
            this.commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
        }

        /// <summary>
        /// Gets all comments.
        /// </summary>
        /// <returns>All comments.</returns>
        /// <response code="200">Returns all comments.</response>
        /// <response code="500">Server error.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComments()
        {
            return await commentService.GetCommentsAsync();
        }

        /// <summary>
        /// Gets comment by id.
        /// </summary>
        /// <param name="commentId">A comment identifier.</param>
        /// <returns>A <see cref="CommentDTO"/>.</returns>
        /// <response code="200">Returns comment.</response>
        /// <response code="404">Returns when comment is not found.</response>
        /// <response code="500">Server error.</response>
        [HttpGet("{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CommentDTO>> GetComment(int commentId)
        {
            var comment = await commentService.GetCommentAsync(commentId);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        /// <summary>
        /// Adds new comment.
        /// </summary>
        /// <param name="createdComment">New comment.</param>
        /// <returns>A <see cref="CreatedAtActionResult"/>.</returns>
        /// <response code="201">Returns the newly created comment.</response>
        /// <response code="400">If the comment is not valid.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="409">If the user with given userId or article with given articleId does not exists.</response>
        /// <response code="500">Server error.</response>
        [HttpPost]
        //  [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateCommentRequest>> AddComment(UpdateCommentRequest createdComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var comment = await commentService.CreateCommentAsync(createdComment);

                return CreatedAtAction("GetComment", new { commentId = comment }, comment);
            }
            catch (ResourceHasConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <param name="updatedComment">A <see cref="CommentDTO"/>.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        /// <response code="204">Comment is updated.</response>
        /// <response code="400">If the comment is not valid.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="404">Returns when comment is not found.</response>
        /// <response code="409">If the user with given userId or article with given articleId does not exists.</response>
        /// <response code="500">Server error.</response>
        [HttpPut("{commentId}")]
        //  [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateComment(int commentId, UpdateCommentRequest updatedComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var comment = await commentService.UpdateCommentAsync(commentId, updatedComment);

                if (comment == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ResourceHasConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an existing comment.
        /// </summary>
        /// <param name="commentId">An comment identifier.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        /// <response code="204">Comment is deleted.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="404">Returns when comment is not found.</response>
        /// <response code="500">Server error.</response>
        [HttpDelete("{commentId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var deletedComment = await commentService.GetCommentAsync(commentId);

            if (deletedComment == null)
            {
                return NotFound();
            }

            await commentService.DeleteCommentAsync(commentId);

            return NoContent();
        }
    }
}