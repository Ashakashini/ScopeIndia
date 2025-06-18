namespace ScopeIndia.Models
{
    public class Otp
    {
        public int Id { get; set; }  // Primary key
        public string Email { get; set; }  // User's email
        public string OTP { get; set; }  // OTP code
        public DateTime ExpiryDate { get; set; }  // Expiry date of OTP
        public DateTime CreatedAt { get; set; }  // When OTP was generated
    }
}
