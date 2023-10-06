using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class UserRegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]       
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
    }
}
