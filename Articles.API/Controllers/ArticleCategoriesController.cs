using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Articles.Data.Articles;
using Articles.Services.ArticleManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Articles.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleCategoriesController : ControllerBase
    {
        private readonly ArticleCategoryService articleCategoryService;

        public ArticleCategoriesController(ArticleCategoryService articleCategoryService)
        {
            this.articleCategoryService = articleCategoryService;
        }

        /// <summary>
        /// Gets all articles categories.
        /// </summary>
        /// <returns>All articles categories.</returns>
        /// <response code="200">Returns all articles categories.</response>
        /// <response code="500">Server error.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ArticleCategoryDTO>>> GetArticleCategories()
        {
            return await articleCategoryService.GetArticleCategoriesAsync();
        }

        /// <summary>
        /// Gets article category by id.
        /// </summary>
        /// <param name="articleCategoryId">An article category identifier.</param>
        /// <returns>A <see cref="ArticleCategoryDTO"/>.</returns>
        /// <response code="200">Returns article category.</response>
        /// <response code="404">Returns when article category is not found.</response>
        /// <response code="500">Server error.</response>
        [HttpGet("{articleCategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ArticleCategoryDTO>> GetArticleCategory(int articleCategoryId)
        {
            var articleCategory = await articleCategoryService.GetArticleCategoryAsync(articleCategoryId);

            if (articleCategory == null)
            {
                return NotFound();
            }

            return articleCategory;
        }

        /// <summary>
        /// Adds new article category.
        /// </summary>
        /// <param name="createdArticleCategory">New article category.</param>
        /// <returns>A <see cref="CreatedAtActionResult"/>.</returns>
        /// <response code="201">Returns the newly created article category.</response>
        /// <response code="400">If the article category is not valid.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="500">Server error.</response>
        [HttpPost]
      //  [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ArticleCategoryDTO>> AddArticleCategory(UpdateArticleCategoryRequest createdArticleCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var articleCategory = await articleCategoryService.CreateArticleCategoryAsync(createdArticleCategory);
           
            return CreatedAtAction("GetArticleCategory", new { articleCategoryId = articleCategory.Id }, articleCategory);
        }

        /// <summary>
        /// Updates an existing article category.
        /// </summary>
        /// <param name="articleCategoryId">The article category identifier.</param>
        /// <param name="updatedArticleCategory">A <see cref="ArticleCategory"/>.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        /// <response code="204">Article category is updated.</response>
        /// <response code="400">If the article category is not valid.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="404">Returns when article category is not found.</response>
        /// <response code="500">Server error.</response>
        [HttpPut("{articleCategoryId}")]
       // [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateArticleCategory(int articleCategoryId, UpdateArticleCategoryRequest updatedArticleCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var articleCategory = await articleCategoryService.UpdateArticleCategoryAsync(articleCategoryId, updatedArticleCategory);

            if (articleCategory == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes an existing article category.
        /// </summary>
        /// <param name="articleCategoryId">An article category identifier.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        /// <response code="204">Article category is deleted.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="404">Returns when article category is not found.</response>
        /// <response code="500">Server error.</response>
        [HttpDelete("{articleCategoryId}")]
      //  [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteArticleCategory(int articleCategoryId)
        {
            var deletedArticleCategory = await articleCategoryService.GetArticleCategoryAsync(articleCategoryId);

            if (deletedArticleCategory == null)
            {
                return NotFound();
            }

            await articleCategoryService.DeleteArticleCategoryAsync(articleCategoryId);

            return NoContent();
        }
    }
}