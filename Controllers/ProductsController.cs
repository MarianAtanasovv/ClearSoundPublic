using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using ClearSoundCompany.Services.Products;

namespace ClearSoundCompany.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductServices _productServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ProductServices productServices, IWebHostEnvironment hostEnvironment)
        {
            _productServices = productServices;
            _webHostEnvironment = hostEnvironment;
        }

        //Error Page
        [Route("/Error404")]
        public IActionResult Error404()
        {
            return View();
        }

        //View for all Categories with added Products inside
        public IActionResult AllCategories()
        {
            var categories = _productServices.AllCategories();
            return View(categories);
        }

        //View for all Products inside specific Category
        [HttpGet]
        public IActionResult AllProductsByCategory(string id)
        {
            var products = _productServices.AllProductsByCategory(id);
            if (products != null)
            {
                return View(products);
            }

            return RedirectToAction("Error404", new {message = "Category NOT found!"});
        }

        //View for all information for specific Product
        [HttpGet]
        public IActionResult ProductInformation(string id)
        {
            var productData = _productServices.ProductInformation(id);
            if (productData != null)
            {
                return View(productData);
            }

            return RedirectToAction("Error404");
        }

        //View for all information for specific Product
        [HttpGet]
        public IActionResult ProductInformationNewProduct(string id)
        {
            var productData = _productServices.ProductInformation(id);
            if (productData != null)
            {
                return View(productData);
            }

            return RedirectToAction("Error404");
        }
        //All NEW Products 
        [HttpGet]
        public IActionResult AllNewProducts()
        {
            var newProducts = _productServices.AllNewProducts();
            return View(newProducts);
        }

        //Select the Product before current in Product Information page
        [HttpGet]
        public IActionResult ProductBefore(string id)
        {
            var beforeProductId = _productServices.ProductBeforeCurrent(id);
            return RedirectToAction("ProductInformation", "Products", new {id = beforeProductId});
        }

        //Select the Product after current in Product Information page
        [HttpGet]
        public IActionResult ProductAfter(string id)
        {
            var nextProductId = _productServices.ProductAfterCurrent(id);

            return RedirectToAction("ProductInformation", "Products", new {id = nextProductId});
        }

        //Select the Product before current in Product Information page
        [HttpGet]
        public IActionResult NewProductBefore(string id)
        {
            var beforeProductId = _productServices.NewProductBeforeCurrent(id);
            return RedirectToAction("ProductInformationNewProduct", "Products", new {id = beforeProductId});
        }

        //Select the Product after current in Product Information page
        [HttpGet]
        public IActionResult NewProductAfter(string id)
        {
            var nextProductId = _productServices.NewProductAfterCurrent(id);

            return RedirectToAction("ProductInformationNewProduct", "Products", new {id = nextProductId});
        }

        //Method for downloading files
        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "database_files/Products/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/pdf", fileName);
        }
    }
}