using System.ComponentModel.DataAnnotations;

namespace ScopeIndia.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage ="Enter the value")]
        [EmailAddress(ErrorMessage ="Please enter a valid email address")]
        public string FromAddress {  get; set; }

        [Required(ErrorMessage ="Enter the value")]
        [EmailAddress(ErrorMessage ="Please enter a valid email address")]
        public string ToAddress {  get; set; }

        [StringLength(100,ErrorMessage ="Please enter the message for your email")]
        public string Subject {  get; set; }

       [StringLength(1500,ErrorMessage ="Please enter valid message")]
        public string Message {  get; set; }
    }
}
