using ClearSoundCompany.Areas.Administration.Models.About;
using ClearSoundCompany.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ClearSoundCompany.Controllers
{
    public class AboutController : Controller
    {
        private readonly ClearSoundDbContext _data;

        public AboutController(ClearSoundDbContext data)
        {
            _data = data;
            
        }

        //Index page
        public IActionResult Info()
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

            return RedirectToAction("Index", "Home");
        }

        public IActionResult FAQ()
        {
            return View();
        }
    }
}