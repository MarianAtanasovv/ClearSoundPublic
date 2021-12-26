using ClearSoundCompany.Data;
using ClearSoundCompany.Data.Models.Rental;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ClearSoundCompany.Services.Festivals
{
    public class FestivalServices
    {
        private readonly ClearSoundDbContext _data;

        public FestivalServices(ClearSoundDbContext data)
        {
            _data = data;
        }

        //View for all Countries with added Festivals inside
        public ICollection<Country> AllCountries()
        {
            var countries = _data
                .Countries
                .Include(i => i.Festivals)
                .OrderBy(i => i.Name)
                .ToList();

            return countries;
        }

        //View for all Festivals inside specific Country
        public ICollection<Festival> AllFestivalsByCountry(string id)
        {
            var festivals = _data
                .Festivals
                .Where(i => i.CountryId == id)
                .Include(i => i.Country)
                .Include(i => i.FestivalImages)
                .ToList();

            return festivals;
        }

        //View for all information for specific Festival
    
        public Festival FestivalInformation(string id)
        {
            var festival = _data
                .Festivals
                .Include(i => i.FestivalImages)
                .Include(i => i.Country)
                .FirstOrDefault(i => i.Id == id);

            return festival;
        }
    }
}