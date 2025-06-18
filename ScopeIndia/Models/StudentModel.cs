using System.ComponentModel.DataAnnotations;

namespace ScopeIndia.Models
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }
    public class StudentModel
    {
        public int ? Id { get; set; }
        public string StudentFirstName {  get; set; }
        public string StudentLastName { get; set; }

       

        [Required(ErrorMessage ="Select the valid gender")]
        
        public string Gender { get; set; }

        [Required(ErrorMessage = "Enter the Valid Date of Birth")]
        [DataType(DataType.DateTime)]

        public DateTime? StudentDateOfBirth {  get; set; }

        [Required(ErrorMessage = "Enter the Valid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string StudentEmail { get; set; }

        [Required(ErrorMessage = "Enter the Valid Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string StudentPhoneNumber { get; set; }
        public string StudentCountry {  get; set; }
        public string StudentState {  get; set; }
        public string StudentCity {  get; set; }
        public string StudentHobbies {  get; set; }
        public IFormFile StudentUploadAvatar {  get; set; }
        public string? Avatarpath {  get; set; }

        public int? CourseId { get; set; }

        public string? password {  get; set; }

        public bool IsVerified {  get; set; }

       

       




    }
}
