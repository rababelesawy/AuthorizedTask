using System.ComponentModel.DataAnnotations;

namespace Authontication.Api.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$" , ErrorMessage = "Minimum 6 characters, at least one uppercase, one lowercase, one digit, and one special character")]
        // T@b1l3x  P@ssw0rd
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
