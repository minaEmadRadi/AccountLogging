namespace Account_Loging.BL.Dtos
{
    public class UserDto
    {
        public int Id { get; set; } 
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
    }
}
