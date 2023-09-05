using Microsoft.AspNetCore.Identity;

namespace ForestAppUI.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoUrl { get; set; }
        public string? AboutAuthor { get; set; }
    }
}
