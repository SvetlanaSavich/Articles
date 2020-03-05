using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Articles.Data;
using Articles.Data.Articles;
using Articles.Services;
using Articles.Services.ArticleManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Articles.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService articleService;

        public ArticlesController(ArticleService articleService)
        {
            this.articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
        }

        /// <summary>
        /// Gets all articles.
        /// </summary>
        /// <returns>All articles.</returns>
        /// <response code="200">Returns all articles.</response>
        /// <response code="500">Server error.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetArticles()
        {
            return await articleService.GetArticlesAsync();
        }

        /// <summary>
        /// Gets article by id.
        /// </summary>
        /// <param name="articleId">An article identifier.</param>
        /// <returns>A <see cref="ArticleDTO"/>.</returns>
        /// <response code="200">Returns article.</response>
        /// <response code="404">Returns when article is not found.</response>
        /// <response code="500">Server error.</response>
        [HttpGet("{articleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ArticleDTO>> GetArticle(int articleId)
        {
            var article = await articleService.GetArticleAsync(articleId);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        /// <summary>
        /// Adds new article.
        /// </summary>
        /// <param name="createdArticle">New article.</param>
        /// <returns>A <see cref="CreatedAtActionResult"/>.</returns>
        /// <response code="201">Returns the newly created article.</response>
        /// <response code="400">If the article is not valid.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="409">If the user with given userId or article category with given categoryId does not exists or article with same title exists.</response>
        /// <response code="500">Server error.</response>
        [HttpPost]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ArticleDTO>> AddArticle(UpdateArticleRequest createdArticle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var article = await articleService.CreateArticleAsync(createdArticle);

                return CreatedAtAction("GetArticle", new { articleId = article.Id }, article);
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
        /// Updates an existing article.
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <param name="updatedArticle">A <see cref="Article"/>.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        /// <response code="204">Article is updated.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="400">If the article is not valid.</response>
        /// <response code="404">Returns when article is not found.</response>
        /// <response code="409">If the user with given userId or article category with given categoryId does not exists.</response>
        /// <response code="500">Server error.</response>
        [HttpPut("{articleId}")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateArticle(int articleId, UpdateArticleRequest updatedArticle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var article = await articleService.UpdateArticleAsync(articleId, updatedArticle);

                if (article == null)
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
        /// Deletes an existing article.
        /// </summary>
        /// <param name="articleId">An article identifier.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        /// <response code="204">Article is deleted.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="404">Returns when article is not found.</response>
        /// <response code="500">Server error.</response>
        [HttpDelete("{articleId}")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteArticle(int articleId)
        {
            var deletedArticle = await articleService.GetArticleAsync(articleId);

            if (deletedArticle == null)
            {
                return NotFound();
            }

            await articleService.DeleteArticleAsync(articleId);

            return NoContent();
        }
    }
}