
using ClearSoundCompany.Areas.Administration.Models.Festivals;
using ClearSoundCompany.Data;
using ClearSoundCompany.Data.Models.Rental;
using ClearSoundCompany.Models.Festivals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ClearSoundCompany.Areas.Administration.Services;

namespace ClearSoundCompany.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Administrator")]
    public class FestivalsController : Controller
    {
        private readonly ClearSoundDbContext _data;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FestivalsController(ClearSoundDbContext data, IWebHostEnvironment hostEnvironment)
        {
            _data = data;

            _webHostEnvironment = hostEnvironment;
        }

        //View for "Adding a Festival" page
        [HttpGet]
        public IActionResult AddFestival()
        {
            return View(new AddFestivalFormModel
            {
                Countries = GetFestivalCountries()
            });
        }

        //Post method for adding a Festival to the database
        [HttpPost]
        public IActionResult AddFestival(AddFestivalFormModel festival)
        {
            if (!_data.Countries.Any(c => c.Id == festival.CountryId))
                ModelState.AddModelError(nameof(festival.CountryId), "Country does not exist!");

            if (!ModelState.IsValid)
            {
                festival.Countries = GetFestivalCountries();
                return View(festival);
            }

            var festivalData = new Festival
            {
                Name = festival.Name,
                Date = DateTime.Parse(festival.Date),
                CountryId = festival.CountryId
            };

            foreach (var image in festival.Images)
            {
                var uploadingFile = new UploadingFile(_webHostEnvironment);

                var uniqueFileName = uploadingFile.UploadFileAndChangeName(image, "Festivals");
                var festivalImageData = new FestivalImage
                {
                    Name = uniqueFileName,
                    FestivalId = festivalData.Id
                };
                _data.FestivalImages.Add(festivalImageData);

                festivalData.FestivalImages.Add(festivalImageData);
            }

            _data.Festivals.Add(festivalData);
            _data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        //View for Editing a specific Festvial
        [HttpGet]
        public IActionResult EditFestival(string id)
        {
            var product = _data.Festivals.FirstOrDefault(i => i.Id == id);

            if (product != null)
            {
                return View(new EditFestivalFormModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Date = product.Date.ToShortDateString(),
                    CountryId = product.CountryId,
                    Countries = GetFestivalCountries()
                });
            }

            return RedirectToAction("AllCountries", "Festivals", new { area = "" });
        }

        //Post method for Editing a Festvial
        [HttpPost]
        public IActionResult EditFestival(EditFestivalFormModel festival)
        {
            if (!_data.Countries.Any(c => c.Id == festival.CountryId))
                ModelState.AddModelError(nameof(festival.CountryId), "Country does not exist!");

            if (!ModelState.IsValid)
            {
                festival.Countries = GetFestivalCountries();
                return View(festival);
            }

            var existingFestival = _data.Festivals.FirstOrDefault(i => i.Id == festival.Id);
            if (existingFestival != null)
            {
                existingFestival.Name = festival.Name;
                existingFestival.CountryId = festival.CountryId;
                if (festival.Date != null)
                {
                    existingFestival.Date = DateTime.Parse(festival.Date);
                }
            }

            //Saving the database
            _data.SaveChanges();

            return RedirectToAction("AllCountries", "Festivals", new { area = "" });
        }

        //Cascade deleting a Festival
        [HttpPost]
        public IActionResult DeleteFestival(string id)
        {
            var festival = _data.Festivals.Include(i => i.FestivalImages).FirstOrDefault(i => i.Id == id);
            if (festival != null)
            {
                _data.Festivals.Remove(festival);
                _data.SaveChanges();
            }

            return RedirectToAction("AllCountries", "Festivals", new { area = "" });
        }

        //Method for returning all Festival Countries from the database
        private ICollection<FestivalCountryViewModel> GetFestivalCountries()
        {
            return _data
                .Countries
                .Select(c => new FestivalCountryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }
    }
}