using System.Collections.Generic;
using Entities.Models;
using Entities.ProjectContext;
using Services.Base;
using Services.Contracts;

namespace Services
{
    public class PostImageService  : BaseService<PostImage>, IPostImageService
    {
        private BlogContext _context; 
        public PostImageService(BlogContext context) : base(context)
        {
            this._context = context;
        }

        public void SetFalse(List<PostImage> images)
        {
            foreach (PostImage image in images)
            {
                image.IsMain = false;
                Update(image);
            }
            Save();
        }
    }
}
