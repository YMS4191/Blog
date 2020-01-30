using System;
using System.Collections.Generic;
using Entities.Core;
using Entities.Enums;

namespace Entities.Models
{
    public class Post : CoreEntity
    {  
        public string Title { get; set; }  
        public string Content { get; set; } 
        public string User { get; set; }
        public ShowType ShowType { get; set; }
         
        public virtual ICollection<PostImage> Images { get; set; }
    }
}
