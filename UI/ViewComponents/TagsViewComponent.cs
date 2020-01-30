using System.Collections.Generic;
using UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace UI.ViewComponents
{
    public class TagsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var tags = new List<Tag>
            {
                new Tag {Id = 1, Name ="İstanbul"},
                new Tag {Id = 1, Name ="Baklava"},
                new Tag {Id = 1, Name ="Poğaça"},
                new Tag {Id = 1, Name ="Zeytinli Açma"},
                new Tag {Id = 1, Name ="Çikolatalı Aç"}
            };
            return View(tags);
        }
    }
}