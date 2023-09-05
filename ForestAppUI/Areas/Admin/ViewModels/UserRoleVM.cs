using ForestAppUI.Models;
using Microsoft.AspNetCore.Identity;


namespace ForestAppUI.Areas.Admin.ViewModels
{
    public class UserRoleVM
    {
        public User User { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
