using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace UI.Areas.Admin.CustomHelpers
{
    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class RoleUserTagHelper : TagHelper
    {
        private readonly UserManager<BlogUser> _userManager;
        private readonly RoleManager<IdentityRole> _identityRole;

        public RoleUserTagHelper(UserManager<BlogUser> userManager, RoleManager<IdentityRole> identityRole)
        {
            this._userManager = userManager;
            this._identityRole = identityRole;
        }


        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            List<string> names = new List<string>();
            var role = await _identityRole.FindByIdAsync(Role);

            if (role != null)
            {
                foreach (var user in _userManager.Users)
                {
                    if (user != null && await _userManager.IsInRoleAsync(user, role.Name))
                        names.Add(user.UserName);
                }
            }
            output.Content.SetContent(names.Count == 0 ? "No users" : string.Join(", ", names));
        }
    }
}
