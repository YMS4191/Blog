using System;
using System.Net;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using UI.Areas.Admin.Utility;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace LayoutApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class PostImagesController : Controller
    {
        private readonly IPostService _postService;
        private readonly IFileUploader _fileUploader;
        private readonly IPostImageService _postImageService;

        public PostImagesController(IPostService postService, IFileUploader fileUploader, IPostImageService postImageService)
        {
            this._postService = postService;
            this._fileUploader = fileUploader;
            this._postImageService = postImageService;
        }

        public IActionResult Upload(Guid id)
        {
            return View(id);
        }

        [HttpPost]
        public IActionResult Upload(IFormFile[] file, Guid? id)
        {
            if (id.HasValue)
            {
                Post post = _postService.GetById(id.Value);

                if (post == null)
                {
                    return NotFound();
                }

                if (file.Length > 0)
                {
                    foreach (var item in file)
                    {
                        var result = _fileUploader.Upload(item);

                        if (result.FileResult == UI.Areas.Admin.Utility.FileResult.Succeded)
                        {
                            PostImage image = new PostImage
                            {
                                ImageUrl = result.FileUrl,
                                PostId = id.Value
                            };

                            _postImageService.Add(image);
                            _postImageService.Save();
                        }
                    }
                }
            }

            return View();
        }




        public IActionResult Index(Guid? id)
        {
            if (id.HasValue)
            {
                return View(id);
            }
            return RedirectToAction("Index", "Posts");
        }

        public IActionResult _Index(Guid? id)
        {
            if (id.HasValue)
            {
                var images = _postImageService.GetByDefault(x => x.PostId == id).Select(x => new
                {

                    x.Id,
                    x.ImageUrl,
                    x.IsMain

                }).ToList();

                if (images.Count > 0)
                {
                    return Json(images);
                }
            }

            return Json(HttpStatusCode.BadRequest);
        }


        public IActionResult _Delete(Guid? id)
        {
            if (id.HasValue)
            {
                var image = _postImageService.GetById(id.Value);
                if (image == null)
                {
                    return Json(HttpStatusCode.NoContent);
                }

                _postImageService.Remove(image);
                _postImageService.Save();
                return Json(HttpStatusCode.OK);
            }

            return Json(HttpStatusCode.BadRequest);
        }


        public IActionResult _Active(Guid? id)
        {
            if (id.HasValue)
            {
                var image = _postImageService.GetById(id.Value);
                if (image == null)
                {
                    return Json(new { Code = HttpStatusCode.NoContent, Result = false }); 
                }
                 
                var images = _postImageService.GetByDefault(x => x.PostId == image.PostId);

                _postImageService.SetFalse(images);

                image.IsMain =  true;

                _postImageService.Update(image);
                _postImageService.Save();
                return Json(new { Code = HttpStatusCode.OK, Result = image.IsMain });
            }
            return Json(new { Code = HttpStatusCode.BadRequest, Result = false }); 
        }
    }
}


