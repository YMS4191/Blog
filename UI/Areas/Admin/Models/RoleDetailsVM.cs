using System.Collections.Generic;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace UI.Areas.Admin.Models
{
    public class RoleDetailsVM
    {

        public IdentityRole Role { get; set; }
        public IEnumerable<BlogUser> Members { get; set; }
        public IEnumerable<BlogUser> NonMembers { get; set; }
    }
}
