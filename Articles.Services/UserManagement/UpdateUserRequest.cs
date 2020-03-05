using System.ComponentModel.DataAnnotations;

namespace Articles.Services.UserManagement
{
    public class UpdateUserRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}