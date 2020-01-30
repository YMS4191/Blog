using Microsoft.AspNetCore.Http;

namespace UI.Areas.Admin.Utility
{

    public enum FileResult
    {
        Succeded = 1,
        Error = 2
    }
     
    public class FileUploadResult
    {
        public string     FileUrl      { get; set; }
        public string     OriginalName { get; set; }
        public string     Base64       { get; set; } 
        public FileResult FileResult   { get; set; } 
        public string     Message      { get; set; } 
    }

    public interface IFileUploader
    {
        public FileUploadResult Upload(IFormFile file);
    }
}



