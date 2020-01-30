using System;
using Microsoft.AspNetCore.Mvc;

namespace LayoutApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }      
    }
}


