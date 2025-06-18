using System.ComponentModel.DataAnnotations;

namespace ScopeIndia.Models
{
    public class LoginModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email {  get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string NewLogin {  get; set; }

        public bool KeepMeLoggedIn { get; set; }

        [DataType(DataType.Password)]
        public string ForgotPassword { get; set; }
    }
}
