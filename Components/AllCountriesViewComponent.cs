using ClearSoundCompany.Data;
using ClearSoundCompany.Data.Models.Rental;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClearSoundCompany.Components
{
    [ViewComponent(Name = "AllCountries")]
    public class AllCountriesViewComponent : ViewComponent
    {
        private readonly List<Country> _countries = new();

        public AllCountriesViewComponent(ClearSoundDbContext data)
        {
            foreach (var country in data.Countries.Include(i => i.Festivals))
            {
                _countries.Add(country);
            }
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _countries;
            return await Task.FromResult((IViewComponentResult)View("Countries", model));
        }
    }
}