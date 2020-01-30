using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace LayoutApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService _postService; 
        public PostsController(IPostService postService)
        {
            this._postService = postService; 
        }

        public IActionResult Index()
        {
            var posts = _postService.GetAll();
          

            return View(posts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                _postService.Add(post);
                bool result = _postService.Save() > 0;
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(post);
        }


        [HttpGet]
        public IActionResult Edit(Guid? id)
        {

            if (id.HasValue)
            {
                var post = _postService.GetById(id.Value);
                if (post == null)
                {
                    return NotFound();
                }

                return View(post);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Edit(Post post)
        {

            if (ModelState.IsValid)
            {
                _postService.Update(post);
                bool result = _postService.Save() > 0;
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(post);

        }


        public IActionResult Details(Guid? id) { return View(); }
        public IActionResult Delete(Guid? id)
        {

            if (id.HasValue)
            {
                var post = _postService.GetById(id.Value);
                if (post == null)
                {
                    return NotFound();
                }

                _postService.Remove(post);
                _postService.Save();
            }

            return RedirectToAction("Index");
        }
    }
}
