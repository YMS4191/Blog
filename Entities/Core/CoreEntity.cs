using System;

namespace Entities.Core
{
    public class CoreEntity : IEntitiy
    {

        public CoreEntity()
        {
            this.Id = Guid.NewGuid();
            this.CreatedDate = DateTime.Now;
        }

        public Guid Id { get; set; } 
        public DateTime CreatedDate { get; set; }
    }
}


