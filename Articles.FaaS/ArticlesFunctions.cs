using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Articles.Data;
using Articles.Services;
using Articles.Services.ArticleManagement;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace Articles.FaaS
{
    public class ArticlesFunctions
    {
        private readonly ArticleService service;

        public ArticlesFunctions(ArticleService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets all articles.
        /// </summary>
        /// <param name="req">A <see cref="HttpRequest"></see></param>
        /// <returns>/>Returns all articles.</returns>
        /// <response code="200">Returns all articles.</response>
        /// <response code="500">Server error.</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<ArticleDTO>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(Error))]
        [FunctionName("GetAllArticles")]
        public async Task<IEnumerable<ArticleDTO>> GetArticles(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "articles")] HttpRequest req)
        {
            return await service.GetArticlesAsync();
        }

        /// <summary>
        /// Gets the article by id.
        /// </summary>
        /// <param name="request">A <see cref="HttpRequest"/>.</param>
        /// <param name="articleId">An article identifier.</param>
        /// <returns>A <see cref="ArticleDTO"/>.</returns>
        /// <response code="200">Returns article.</response>
        /// <response code="404">Returns when article is not found.</response>
        /// <response code="500">Server error.</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ArticleDTO))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(Error))]
        [FunctionName("GetArticle")]
        public async Task<IActionResult> GetArticle(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "articles/{articleId}")]
            HttpRequest request, int articleId)
        {
            var article = await service.GetArticleAsync(articleId);

            if (article == null)
            {
                return new NotFoundObjectResult($"Article with id: {articleId} not found.");
            }

            return new OkObjectResult(article);
        }

        /// <summary>
        /// Adds new article.
        /// </summary>
        /// <param name="request">A <see cref="HttpRequest"/></param>
        /// <returns>A <see cref="ActionResult{ArticleDTO}"/>.</returns>
        /// <response code="200">Returns the newly created article.</response>
        /// <response code="400">If the article is not valid.</response>
        /// <response code="409">If the user with given userId or article category with given categoryId does not exists or article with same title exists.</response>
        /// <response code="500">Server error.</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ArticleDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Conflict, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(Error))]
        [FunctionName("AddArticle")]
        public async Task<ActionResult<ArticleDTO>> AddArticle([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "articles")][RequestBodyType(typeof(UpdateArticleRequest), "product request")]
            HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();

            var createdArticle = JsonConvert.DeserializeObject<UpdateArticleRequest>(requestBody);

            if (!createdArticle.IsValid(validationResults: out var validationResults))
            {
                return new BadRequestObjectResult($"{nameof(ArticleDTO)} is invalid: {string.Join(", ", validationResults.Select(s => s.ErrorMessage))}");
            }

            try
            {
                var article = await service.CreateArticleAsync(createdArticle);

                return new OkObjectResult(article);
            }
            catch (ResourceHasConflictException ex)
            {
                return new ConflictObjectResult(ex.Message);
            }
            catch (ResourceNotFoundException ex)
            {
                return new NotFoundObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing article.
        /// </summary>
        /// <param name="request">A <see cref="HttpRequest"/></param>
        /// <param name="articleId">An updating article id.</param>
        /// <returns>A <see cref="ActionResult{ArticleDTO}"/>.</returns>
        /// <response code="204">Article is updated.</response>
        /// <response code="400">If the article is not valid.</response>
        /// <response code="404">Returns when article is not found.</response>
        /// <response code="409">If the user with given userId or article category with given categoryId does not exists.</response>
        /// <response code="500">Server error.</response>
        [ProducesResponseType((int)HttpStatusCode.NoContent, Type = typeof(int))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Conflict, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(Error))]
        [FunctionName("UpdateArticle")]
        public async Task<ActionResult<ArticleDTO>> UpdateArticle([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "articles/{articleId}")][RequestBodyType(typeof(UpdateArticleRequest), "product request")]
            HttpRequest request, int articleId)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();

            var createdArticle = JsonConvert.DeserializeObject<UpdateArticleRequest>(requestBody);

            if (!createdArticle.IsValid(validationResults: out var validationResults))
            {
                return new BadRequestObjectResult($"{nameof(ArticleDTO)} is invalid: {string.Join(", ", validationResults.Select(s => s.ErrorMessage))}");
            }

            try
            {
                var article = await service.UpdateArticleAsync(articleId, createdArticle);

                if (article == null)
                {
                    return new NotFoundObjectResult($"Article with id: {articleId} not found.");
                }

                return new NoContentResult();
            }
            catch (ResourceHasConflictException ex)
            {
                return new ConflictObjectResult(ex.Message);
            }
            catch (ResourceNotFoundException ex)
            {
                return new NotFoundObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an existing article.
        /// </summary>
        /// <param name="request">A <see cref="HttpRequest"/>.</param>
        /// <param name="articleId">The article identifier.</param>
        /// <returns>A <see cref="IActionResult"/>.</returns>
        /// <response code="204">Article is deleted.</response>
        /// <response code="404">Returns when article is not found.</response>
        /// <response code="500">Server error.</response>
        [ProducesResponseType((int)HttpStatusCode.NoContent, Type = typeof(int))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(Error))]
        [FunctionName("DeleteArticle")]
        public async Task<IActionResult> DeleteArticle(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "articles/{articleId}")]
            HttpRequest request, int articleId)
        {
            var deletedArticle = await service.GetArticleAsync(articleId);

            if (deletedArticle == null)
            {
                return new NotFoundObjectResult($"Article with id: {articleId} not found.");
            }

            await service.DeleteArticleAsync(articleId);

            return new NoContentResult();
        }
    }
}