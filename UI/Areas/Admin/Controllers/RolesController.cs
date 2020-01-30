using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public RolesController(
            RoleManager<IdentityRole> roleManager,
            UserManager<BlogUser> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }


        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(name.ToLower()));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            ModelState.AddModelError("", "role not found");
            return View(name);
        }


        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            var members = new List<BlogUser>();
            var nonMembers = new List<BlogUser>();


            foreach (var user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }

            var model = new RoleDetailsVM
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(RoleEditModelVM vm)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                // Role Kullanıcı Ekleme
                foreach (var userID in vm.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userID);

                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user, vm.RoleName);

                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);

                            }
                        }
                    }
                }

                // Rolden Kullanıcı Silme
                foreach (var userID in vm.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userID);

                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, vm.RoleName);

                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);

                            }
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit", vm.Id);
            }
        }
    }
}