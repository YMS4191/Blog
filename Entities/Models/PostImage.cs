using System;
using Entities.Core;

namespace Entities.Models
{
    public class PostImage : CoreEntity
    {

        public PostImage()
        {
            this.IsMain = false;
        }

        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }

        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
