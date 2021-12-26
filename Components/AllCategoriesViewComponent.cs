using ClearSoundCompany.Data;
using ClearSoundCompany.Data.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClearSoundCompany.Components
{
    [ViewComponent(Name = "AllCategories")]
    public class AllCategoriesViewComponent : ViewComponent
    {
        private readonly List<Category> _categories = new();

        public AllCategoriesViewComponent(ClearSoundDbContext data)
        {
            foreach (var category in data.Categories.Include(i => i.Products))
            {
                _categories.Add(category);
            }
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _categories;
            return await Task.FromResult((IViewComponentResult)View("Categories", model));
        }
    }
}