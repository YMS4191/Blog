using System.Collections.Generic;
using Entities.Models;
using Services.Core;

namespace Services.Contracts
{
    public interface IPostImageService : IService<PostImage>
    { 
        void SetFalse(List<PostImage> images);
    }
}
