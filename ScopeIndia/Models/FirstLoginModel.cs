using System.ComponentModel.DataAnnotations;

namespace ScopeIndia.Models
{
    public class FirstLoginModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        
    }
}
