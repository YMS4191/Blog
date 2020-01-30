using System.Collections.Generic;
using UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace UI.ViewComponents
{
    public class MenusViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var menus = new List<Menu>()
            {
                new Menu{ Id = 1, Name = "Anasayfa", Controller = "Home", Action = "Index" },
                new Menu{ Id = 2, Name = "Hakkımızda", Controller = "Home", Action = "About" }, 
                new Menu{ Id = 3, Name = "İletişim", Controller = "Home", Action = "Contact" } 
            };
            return View(menus);
        }
    }
}