using ClearSoundCompany.Areas.Administration.Models.About;
using ClearSoundCompany.Data;
using ClearSoundCompany.Data.Models.About;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace ClearSoundCompany.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class AboutController : Controller
    {
        private readonly ClearSoundDbContext _data;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AboutController(ClearSoundDbContext data, IWebHostEnvironment hostEnvironment)
        {
            this._data = data;
            _webHostEnvironment = hostEnvironment;
        }

        //View for Administrator, for editing About information and image
        public IActionResult Edit()
        {
            var about = _data.About.FirstOrDefault();
            if (about != null)
            {
                var newModel = new AddOrEditFormModel
                {
                    Description = about.Description,
                    ImageName = about.Image
                };
                return View(newModel);
            }

            return View();
        }

        //Post for Administrator, for editing About information and image
        [HttpPost]
        public IActionResult Edit(AddOrEditFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Info", "About", new { area = "" });
            }

            var about = _data.About.FirstOrDefault();
            if (about == null)
            {
                var newModel = new About()
                {
                    Description = model.Description,
                    Image = UploadedFile(model.Image),
                };
                _data.About.Add(newModel);
            }
            else
            {
                about.Description = model.Description;
                if (model.Image != null)
                {
                    about.Image = UploadedFile(model.Image);
                }
            }
            _data.SaveChanges();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        //Method for changing file names and returning that name (string)
        private string UploadedFile(IFormFile image)
        {
            string uniqueFileName = null;

            if (image != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img");
                uniqueFileName = Guid.NewGuid() + "_" + image.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                image.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}