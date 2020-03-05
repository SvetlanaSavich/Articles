using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Articles.Services;
using Articles.Services.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Articles.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService userService;

        public UsersController(UserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>All users.</returns>
        /// <response code="200">Returns all users.</response>
        /// <response code="500">Server error.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await userService.GetUsersAsync();
        }

        /// <summary>
        /// Gets user by id.
        /// </summary>
        /// <param name="userId">An user identifier.</param>
        /// <returns>A <see cref="UserDTO"/>.</returns>
        /// <response code="200">Returns user.</response>
        /// <response code="404">Returns when user is not found.</response>
        /// <response code="500">Server error.</response>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDTO>> GetUser(int userId)
        {
            var user = await userService.GetUserAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// Adds new user.
        /// </summary>
        /// <param name="createdUser">New user.</param>
        /// <returns>A <see cref="CreatedAtActionResult"/>.</returns>
        /// <response code="201">Returns the newly created user.</response>
        /// <response code="400">If the user is not valid.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="500">Server error.</response>
        [HttpPost]
      //  [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateUserRequest>> AddUser(UpdateUserRequest createdUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var user = await userService.CreateUserAsync(createdUser);

                return CreatedAtAction("GetUser", new { userId = user.Id }, user);
            }
            catch (ResourceHasConflictException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="updatedUser">A <see cref="UserDTO"/>.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        /// <response code="204">User is updated.</response>
        /// <response code="400">If the user is not valid.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="404">Returns when user is not found.</response>
        /// <response code="500">Server error.</response>
        [HttpPut("{userId}")]
      //  [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserRequest updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var user = await userService.UpdateUserAsync(userId, updatedUser);

                if (user == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ResourceHasConflictException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an existing user.
        /// </summary>
        /// <param name="userId">An user identifier.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        /// <response code="204">User is deleted.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="404">Returns when user is not found.</response>
        /// <response code="500">Server error.</response>
        [HttpDelete("{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var deletedUser = await userService.GetUserAsync(userId);

            if (deletedUser == null)
            {
                return NotFound();
            }

            await userService.DeleteUserAsync(userId);

            return NoContent();
        }
    }
}