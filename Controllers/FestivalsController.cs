using Microsoft.AspNetCore.Mvc;
using ClearSoundCompany.Services.Festivals;

namespace ClearSoundCompany.Controllers
{
    public class FestivalsController : Controller
    {
        private readonly FestivalServices _festivalServices;

        public FestivalsController(FestivalServices festivalServices)
        {
            _festivalServices = festivalServices;
        }

        //View for all Countries with added Festivals inside
        [HttpGet]
        public IActionResult AllCountries()
        {
            var countries = _festivalServices.AllCountries();

            return View(countries);
        }

        //View for all Festivals inside specific Country
        [HttpGet]
        public IActionResult AllFestivalsByCountry(string id)
        {
            var festivals = _festivalServices.AllFestivalsByCountry(id);

            return View(festivals);
        }

        //View for all information for specific Festival
        [HttpGet]
        public IActionResult FestivalInformation(string id)
        {
            var festival = _festivalServices.FestivalInformation(id);

            return View(festival);
        }
    }
}