using System.ComponentModel.DataAnnotations;

namespace ScopeIndia.Models
{
    public class PasswordSettingModel
    {
        [Required(ErrorMessage ="This field is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 4 characters.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{4,}$",
        ErrorMessage = "Password must contain uppercase, lowercase, and number.")]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
