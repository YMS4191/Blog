using System.Collections.Generic;
using System.Linq;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Services.Contracts;

namespace UI.CustomHelpers
{ 
    [HtmlTargetElement("article", Attributes = "show-type")]
    public class ShowTypeTagHelper : TagHelper
    {
        private readonly IPostImageService _postImageService;
        public ShowTypeTagHelper(IPostImageService  postImageService)
        {
            this._postImageService = postImageService;
        }
         
        [HtmlAttributeName("show-type")]
        public Post Post { get; set; }

        private string ImageUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            List<PostImage> images = null;

            string template = "";

            switch (Post.ShowType)
            {
                case ShowType.none:
                    break;
                case ShowType.standart:

                    images= _postImageService.GetByDefault(x => x.PostId == Post.Id && x.IsMain == true); 
                    if (images.Count > 0)
                    {
                        ImageUrl = images.FirstOrDefault(x => x.PostId == Post.Id && x.IsMain == true).ImageUrl;
                    }
                    else
                    {
                        ImageUrl = "/images/upload/post-img01.jpg";
                    }

                    template = @$"<div class='post-img img-div-cover'>
        <div class='post-list-category'>
            <ul>
                <li><a href='#'>Vestibulum</a></li>
                <li><a href='#'>Aenean</a></li>
            </ul>
        </div>
        <a href='#'>
            <figure>
                <div class='overlay-hover'></div>
                <img src='{ImageUrl}' alt='post image' />
            </figure>
        </a>
    </div> ";

                    break;
                case ShowType.gallery:
                    break;
                case ShowType.slide:


                images = _postImageService.GetByDefault(x => x.PostId == Post.Id );



                    template = $@"
    <div class='post-img'>
        <div class='post-list-category'>
            <ul>
                <li><a href='#'>Vestibulum</a></li>
                <li><a href='#'>Aenean</a></li>
            </ul>
        </div>

        
        <div class='post-list-slide'>
            <div class='flexslider'>
                <ul class='slides'>";


                    foreach (var item in images.OrderByDescending(x => x.IsMain))
                    {  
                        template += @$"  <li>
                        <a class='fancybox' data-fancybox-group='group2' href='{item.ImageUrl}'>
                            <figure>
                                <div class='overlay-hover'></div>
                                <img src='{item.ImageUrl}' alt='post slide' />
                            </figure>
                        </a>
                        </li>";
                    }

                  
                 template += @$" 

                </ul>
            </div>
        </div>  
    </div>
 
    <div class='flexControl-custom'>
        <ol class='flexControl-custom-nav'>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
        </ol>
    </div> ";
                    break;
                case ShowType.video:
                    break;
                case ShowType.audio:
                    break;
                default:
                    break;
            }


            template += @$"<div class='post-entry'>
        <h2><a href='#'>{Post.Title}</a></h2>
        <div class='post-description'>
            <p>
                {Post.Content}
            </p>
        </div>
        <div class='post-meta'>
            <ul>
                <li><a href='#'>{Post.CreatedDate}</a></li>
                <li><a href='#'>John Doe</a></li>
                <li><a href='#'>Comments<span>4</span></a></li>
            </ul>
        </div>
    </div>
    <div class='post-share'>
        <div class='read-more-btn'><a href='#'>Read more</a></div>
        <div class='share-btn'>Share</div>
        <ul class='share-standard'>
            <li><a href='#'><i class='fa fa-facebook'></i></a></li>
            <li><a href='#'><i class='fa fa-twitter'></i></a></li>
            <li><a href='#'><i class='fa fa-google'></i></a></li>
            <li><a href='#'><i class='fa fa-pinterest-p'></i></a></li>
        </ul>
    </div>";

            output.Content.SetHtmlContent(template);
        }
    }
}
