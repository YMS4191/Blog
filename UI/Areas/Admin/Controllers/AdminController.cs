using System;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Entities.ProjectContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly BlogContext _context;
        private readonly UserManager<BlogUser> _userManager;
        private readonly IPasswordHasher<BlogUser> _passwordHasher;
        private readonly IPasswordValidator<BlogUser> _passwordValidator;

        public AdminController(
            BlogContext context,
            UserManager<BlogUser> userManager,
            IPasswordHasher<BlogUser> passwordHasher,
            IPasswordValidator<BlogUser> passwordValidator)
        {
            _context = context;
            this._userManager = userManager;
            this._passwordHasher = passwordHasher;
            this._passwordValidator = passwordValidator;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserVM vm)
        {
            if (ModelState.IsValid)
            {
                BlogUser user = new BlogUser();
                user.UserName = vm.UserName;
                user.Email = vm.Email;
                user.PhoneNumber = vm.PhoneNumber;

                var result = await _userManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(vm);
        }




        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id.HasValue)
            {
                var user = await _userManager.FindByIdAsync(id.Value.ToString());
                if (user != null)
                {
                    UserVM vm = new UserVM();
                    vm.UserName = user.UserName;
                    vm.PhoneNumber = user.PhoneNumber;
                    vm.Password = user.PasswordHash;
                    vm.Email = user.Email;
                    return View(vm);
                }

                return RedirectToAction("Index");
            }
            return View("Index", _userManager.Users);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UserVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(vm.Id.ToString());

                if (user != null)
                {
                    user.Email = vm.Email;
                    user.PhoneNumber = vm.PhoneNumber;
                    IdentityResult validPassword = null;

                    if (!string.IsNullOrWhiteSpace(vm.Password))
                    {
                        validPassword = await _passwordValidator.ValidateAsync(_userManager, user, vm.Password);
                        if (validPassword.Succeeded)
                        {
                            user.PasswordHash = _passwordHasher.HashPassword(user, vm.Password);
                        }
                        else
                        {
                            foreach (var item in validPassword.Errors)
                            {
                                ModelState.AddModelError("", item.Description);
                            }
                        }
                    }

                    if (validPassword.Succeeded)
                    {
                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            foreach (var item in result.Errors)
                            {
                                ModelState.AddModelError("", item.Description);
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "user not found");
                }
            }

            return View("Index", _userManager.Users); 
        }

         

        [HttpPost]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id.HasValue)
            {
                var user = await _userManager.FindByIdAsync(id.Value.ToString());
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "user not found");
                }
            }

            return View("Index", _userManager.Users);
        }
    }
}
