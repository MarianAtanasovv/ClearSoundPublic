using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ClearSoundCompany.Areas.Administration.Services
{
    public class UploadingFile
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadingFile(IWebHostEnvironment hostEnvironment)
        {
            _webHostEnvironment = hostEnvironment;
        }

        public string UploadFileAndChangeName(IFormFile model, string folder)
        {
            string uniqueFileName = null;

            if (model != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, $"database_files/{folder}");
                uniqueFileName = Guid.NewGuid() + "_" + model.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
        public string UploadFile(IFormFile model, string folder)
        {
            string uniqueFileName = null;

            if (model != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, $"database_files/{folder}");
                uniqueFileName = model.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}