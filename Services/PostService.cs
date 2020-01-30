using Entities.Models;
using Entities.ProjectContext;
using Services.Base;
using Services.Contracts;

namespace Services
{
    public class PostService  : BaseService<Post>, IPostService
    {
        private BlogContext _context;
        public PostService(BlogContext context) : base(context)
        {
            this._context = context;
        }
    }
}
