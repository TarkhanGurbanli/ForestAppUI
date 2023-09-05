using System.ComponentModel.DataAnnotations;

namespace ForestAppUI.Dtos
{
    public class RegisterDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string PasswordConfirmation { get; set; }
    }
}
