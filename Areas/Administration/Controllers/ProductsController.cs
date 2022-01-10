using ClearSoundCompany.Areas.Administration.Models.Products;
using ClearSoundCompany.Data;
using ClearSoundCompany.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ClearSoundCompany.Areas.Administration.Services;

namespace ClearSoundCompany.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private readonly ClearSoundDbContext _data;
        private readonly ProductAdminServices _productServices;

        public ProductsController(ProductAdminServices productServices, ClearSoundDbContext data)
        {
            _productServices = productServices;
            _data = data;
        }
        //View for "Adding a Product" page
        public IActionResult AddProduct()
        {
            return View(new AddProductFormModel
            {
                Categories = GetProductCategories()
            });
        }

        //Post method for adding a Product to the database
        [HttpPost]
        public IActionResult AddProduct(AddProductFormModel product)
        {
            if (!_data.Categories.Any(p => p.Id == product.CategoryId))
                ModelState.AddModelError(nameof(product.CategoryId), "Category does not exist!");

            if (!ModelState.IsValid)
            {
                product.Categories = GetProductCategories();
                return View(product);
            }

            _productServices.AddProduct(product);


            return RedirectToAction("AllCategories", "Products", new {area = ""});
        }

        //View for Editing a specific Product
        [HttpGet]
        public IActionResult EditProduct(string id)
        {
            var product = _productServices.PreviewAllInformationForEditing(id);

            if (product != null)
            {
                return View(product);
            }

            return RedirectToAction("Error404");
        }

        //Post method for Editing a Product
        [HttpPost]
        public IActionResult EditProduct(EditProductFormModel product)
        {
            if (!_data.Categories.Any(p => p.Id == product.CategoryId))
                ModelState.AddModelError(nameof(product.CategoryId), "Category does not exist!");

            if (!ModelState.IsValid)
            {
                product.Categories = GetProductCategories();
                return View(product);
            }
            //If everything is okey, all file names are renamed with some random ones
            _productServices.EditProduct(product);

            return RedirectToAction("ProductInformation", "Products", new {id = product.Id, area = ""});
        }

        //View for Deleting image
        [HttpGet]
        public IActionResult AllImagesForProduct(string id)
        {
            var images = _productServices
                .AllImagesForProduct(id)
                .ProductImages
                .OrderBy(i => i.Name);

            if (images.Any()) return View(images);

            TempData["alertWarning"] = "There is no photos to delete :(";
            return RedirectToAction("ProductInformation", "Products", new { id, area = "" });
        }

        //View for all Categories (for editing)
        [HttpGet]
        public IActionResult AllCategoriesForEditAndDelete()
        {
            var product = _productServices.AllCategoriesForEditAndDelete();
            return View(product);
        }

        //View for Adding a Category
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        //Post method for Adding a Category
        [HttpPost]
        public IActionResult AddCategory(AddCategoryFormModel category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if (_data.Categories.Any(i => i.Name == category.Name))
            {
                return View(category);
            }

            _productServices.AddCategory(category);
            return RedirectToAction("AllCategoriesForEditAndDelete", "Products");
        }

        //View for Editing specific Category
        [HttpGet]
        public IActionResult EditCategories(string id)
        {
            var category = _productServices.EditCategories(id);
            if (category == null)
            {
                return RedirectToAction("Error404");
            }

            return View(category);
        }

        //Post method for Editing specific Category
        [HttpPost]
        public IActionResult EditCategories(EditCategoryFormModel category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _productServices.EditCategories(category);

            return RedirectToAction("AllCategoriesForEditAndDelete", "Products");
        }

        //Cascade deleting a Product
        [HttpPost]
        public IActionResult DeleteProduct(string id)
        {
            _productServices.DeleteProduct(id);
            return RedirectToAction("AllCategories", "Products", new {area = ""});
        }

        //Cascade deleting an Image
        [HttpPost]
        public IActionResult DeleteImage(string id, string productId)
        {
            _productServices.DeleteImage(id);
            return RedirectToAction("AllImagesForProduct", "Products", new {id = productId});
        }

        //Cascade deleting all Images
        [HttpPost]
        public IActionResult DeleteAllImages(string id)
        {
           var productId= _productServices.DeleteAllImages(id);
            return RedirectToAction("ProductInformation", "Products", new { id = productId, area = "" });
        }

        //Cascade deleting a Category
        public IActionResult DeleteCategory(string id)
        {
            _productServices.DeleteCategory(id);
            return RedirectToAction("AllCategoriesForEditAndDelete", "Products", new {area = ""});
        }

        //----------------Methods------------------

        //Method for returning all Product Categories from the database
        private ICollection<ProductCategoryViewModel> GetProductCategories()
        {
            return _data
                .Categories
                .Select(c => new ProductCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }
    }
}