using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace UI.Areas.Admin.Utility
{
    public class FileSystemFileUploader : IFileUploader
    {
        private readonly string _filePath;
        public FileSystemFileUploader(string filePath)
        {
            this._filePath = filePath;
        }
        public FileSystemFileUploader()
        {
            this._filePath = "images";
        }
        public FileUploadResult Upload(IFormFile file)
        {
            FileUploadResult result = new FileUploadResult();
            result.FileResult = FileResult.Error;
            result.Message = "Dosya yükleme sırasında bir hata oluştu";

            if (file.Length > 0)
            {

                //     var fileName =  Path.GetFileName(file.FileName)  ; // .jpg, .png, .gif vs...


                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}"; // .jpg, .png, .gif vs...


                //var fileName =$"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}"; // .jpg, .png, .gif vs...
                // eğer kullanıcı dosya eklediyse, dosyanın yüklenirken default ismini alıyoruz.
                result.OriginalName = file.FileName;

                // sunucudaki projenin çalışma yolu
                var physicalPath = Path.Combine(Directory.GetCurrentDirectory(),$"wwwroot/{_filePath}" , fileName);


                if (File.Exists(physicalPath))
                {
                    result.Message = "Böyle bir dosya mevcut!";
                }
                else
                {
                    // dosyanık eklendiği dizin
                    result.FileUrl = $"/{_filePath}/{fileName}";
                    result.Base64 = null;  // fileupload yaptığımızdan, Base64 çevirme işlemi yapılmayacaktır.

                    // yeni bir dosya yükleme için FileStream açtık ve dosyayı Ram üzerinde oluşturduk
                    // stram nesneleri, byte[] şeklinde dosyaları Ram üzerinde tutmamızı sağlayan sınıflardır.

                    // bir nesneyi using() içerisinde kullanabilmeniz için o nesnenin öutlaka IDisposable nesnesinden miras almış olması gerekir. eğer bu şekil bir kullanım şekli yaparsanız GB  collector ram üzerinden nesneyi temizlemesini bekelemezsiniz. İşlem bittiğinide RAM üzerinden direk olarak silinecektir.

                    
                    try
                    {
                        using var stream = new FileStream(physicalPath, FileMode.Create);
                        file.CopyTo(stream);
                        result.FileResult = FileResult.Succeded;
                        result.Message = "Dosya başarılı bir şekilde yüklendi";
                    }
                    catch 
                    {
                        result.FileResult = FileResult.Error;
                    }
                }
            }

            return result;
        }
    }
}


