using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Entities.Models;
using Entities.ProjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;

namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly BlogContext _context;
        private readonly IPostImageService _postImageService;

        public PostsController(IPostService postService, BlogContext blogContext, IPostImageService postImageService)
        {
            this._context = blogContext;
            this._postService = postService;
            this._postImageService = postImageService;
        }


         

        [HttpGet]
        public IActionResult GetPosts()
        { 
            var posts = _postService.GetAll().ToList();
            List<Post> _posts = new List<Post>();

            foreach (Post post in posts)
            {
                var images = _postImageService.GetByDefault(x => x.PostId == post.Id);
                if (images != null)
                {
                    post.Images = new List<PostImage>();
                    post.Images = images;
                }
                _posts.Add(post);
            }

            var items = _posts.Select(x => new
            { 
                x.Id,
                x.Content,
                x.CreatedDate,
                x.Title,
                images = x.Images.Select(i => new { i.ImageUrl })
            });

            return Ok(items);
        }

        // GET: api/Posts/5Ï
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(Guid id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(Guid id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Posts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(Guid id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return post;
        }

        private bool PostExists(Guid id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
