using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Contracts;
using UI.Models;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly IPostImageService _postImageService;


        public HomeController(ILogger<HomeController> logger, IPostService postService, IPostImageService postImageService)
        {
            this._logger = logger;
            this._postService = postService;
            this._postImageService = postImageService;
        }

        public IActionResult Index()
        {
            var posts = _postService.GetByDefault(x => x.ShowType != Entities.Enums.ShowType.none);
            var images = _postImageService.GetAll();
            var tuple = Tuple.Create(posts, images);
            return View(tuple);
        }

        public IActionResult Privacy() { return View(); }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
