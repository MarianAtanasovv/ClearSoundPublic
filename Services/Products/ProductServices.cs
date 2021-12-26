using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ClearSoundCompany.Data;
using ClearSoundCompany.Data.Models.Products;
using ClearSoundCompany.Models.Products;
using ClearSoundCompany.Models.Products.Enum;
using Microsoft.EntityFrameworkCore;

namespace ClearSoundCompany.Services.Products
{
    public class ProductServices
    {
        private readonly ClearSoundDbContext _data;

        public ProductServices(ClearSoundDbContext data)
        {
            _data = data;
        }

        public ICollection<Product> AllProductsByCategory(string id)
        {
            return _data
                .Products
                .Where(i => i.CategoryId == id)
                .Include(i => i.Category)
                .Include(i => i.ProductImages)
                .OrderBy(i => i.Name)
                .ToList();
        }

        public ICollection<Category> AllCategories()
        {
            return _data
                .Categories
                .Include(i => i.Products)
                .ThenInclude(i => i.ProductImages)
                .ToList();
        }

        public ICollection<Product> AllNewProducts()
        {
            return _data
                .Products
                .Where(i => i.New)
                .Include(i => i.Category)
                .Include(i => i.ProductImages)
                .ToList();
        }

        public string ProductBeforeCurrent(string id)
        {
            var currentProduct =
                _data
                    .Products
                    .FirstOrDefault(i => i.Id == id);

            var allProductsSameCategory =
                _data
                    .Categories
                    .Include(i => i.Products)
                    .FirstOrDefault(i => i.Id == currentProduct.CategoryId);

            var allProductsOrdered = allProductsSameCategory
                .Products
                .OrderBy(i => i.Name);

            var beforeProductId = string.Empty;

            if (allProductsOrdered.Count() > 1)
            {
                foreach (var product in allProductsOrdered)
                {
                    if (product.Id == currentProduct.Id)
                    {
                        break;
                    }

                    beforeProductId = product.Id;
                }

                if (currentProduct == allProductsOrdered.FirstOrDefault())
                {
                    beforeProductId = allProductsOrdered.LastOrDefault().Id;
                }
            }
            else
            {
                beforeProductId = id;
            }

            return beforeProductId;
        }

        public string ProductAfterCurrent(string id)
        {
            var currentProduct = _data
                .Products
                .FirstOrDefault(i => i.Id == id);

            var allProductсSameCategory = _data
                .Categories
                .Include(i => i.Products)
                .FirstOrDefault(i => i.Id == currentProduct.CategoryId);

            var allProductsOrdered = allProductсSameCategory
                .Products
                .OrderBy(i => i.Name);

            var nextProductId = string.Empty;

            var foundNextProduct = false;

            if (allProductsOrdered.Count() > 1)
            {
                foreach (var product in allProductsOrdered)
                {
                    if (foundNextProduct)
                    {
                        nextProductId = product.Id;
                        foundNextProduct = false;
                        break;
                    }

                    if (product.Id == currentProduct.Id)
                    {
                        foundNextProduct = true;
                    }
                }

                if (foundNextProduct)
                {
                    nextProductId = allProductsOrdered.FirstOrDefault().Id;
                }
            }
            else
            {
                nextProductId = id;
            }

            return nextProductId;
        }

        public string NewProductBeforeCurrent(string id)
        {
            var currentProduct =
                _data
                    .Products
                    .FirstOrDefault(i => i.Id == id);

            var allNewProducts =
                _data
                    .Products
                    .Where(i => i.New)
                    .OrderBy(i=>i.Name)
                    .ToList();
            

            var beforeProductId = string.Empty;

            if (allNewProducts.Count > 1)
            {
                foreach (var product in allNewProducts)
                {
                    if (product.Id == currentProduct.Id)
                    {
                        break;
                    }

                    beforeProductId = product.Id;
                }

                if (currentProduct == allNewProducts.FirstOrDefault())
                {
                    beforeProductId = allNewProducts.LastOrDefault().Id;
                }
            }
            else
            {
                beforeProductId = id;
            }

            return beforeProductId;
        }

        public string NewProductAfterCurrent(string id)
        {
            var currentProduct =
                _data
                    .Products
                    .FirstOrDefault(i => i.Id == id);

            var allNewProducts =
                _data
                    .Products
                    .Where(i => i.New)
                    .OrderBy(i => i.Name)
                    .ToList();


            var nextProductId = string.Empty;
            var foundNextProduct = false;

            if (allNewProducts.Count > 1)
            {
                foreach (var product in allNewProducts)
                {
                    if (foundNextProduct)
                    {
                        nextProductId = product.Id;
                        foundNextProduct = false;
                        break;
                    }

                    if (product.Id == currentProduct.Id)
                    {
                        foundNextProduct = true;
                    }
                }

                if (foundNextProduct)
                {
                    nextProductId = allNewProducts.FirstOrDefault().Id;
                }
            }
            else
            {
                nextProductId = id;
            }

            return nextProductId;
        }

        public ProductInformationView ProductInformation(string id)
        {
            var productData = _data
                .Products
                .Include(i => i.Category)
                .Include(i => i.Specifications)
                .Include(i => i.ProductImages)
                .FirstOrDefault(i => i.Id == id);

            if (productData == null)
            {
                return null;
            }

            var colorCollection = new List<string>();
            var specs = productData.Specifications.OrderBy(i => i.Order).ToList();
            var product = new ProductInformationView
            {
                Id = productData.Id,
                Name = productData.Name,
                Description = productData.Description,
                ProfileImage = productData.ProfileImage,
                Price = productData.Price,
                Category = productData.Category,
                CategoryId = productData.CategoryId,
                Specifications = specs,
                ProductImages = productData.ProductImages,
                EaseData = productData.EaseData,
                ProductData = productData.ProductData,
                Manual = productData.Manual,
                Colors = colorCollection,
                New = productData.New
            };

            if (productData.ProductImages.Any())
            {
                var regex = Regex.Match(productData.ProductImages.FirstOrDefault().Name,
                    @"(?<Name>\w*)_(?<Color>\w*).(?<Index>\d*).(?<Extension>\w*)");

                foreach (var image in productData.ProductImages)
                {
                    var regexImage = Regex.Match(image.Name,
                        @"(?<Name>\w*)_(?<Color>\w*).(?<Index>\d*).(?<Extension>\w*)");

                    var regColor = regexImage.Groups["Color"].ToString().ToLower();
                    if (Enum.TryParse(regColor, out Color color))
                    {
                        if (!colorCollection.Contains(regColor))
                        {
                            colorCollection.Add(regColor);
                        }
                    }
                    else
                    {
                        colorCollection.Add(regColor);
                    }
                }

                product.ImageName = regex.Groups["Name"].ToString();
                product.Extension = regex.Groups["Extension"].ToString();
            }
            else
            {
                var newImage = new ProductImage
                {
                    Name = "noimage.png"
                };
                product.ProductImages.Add(newImage);
                product.ImageName = "noimage";
                product.Extension = "png";
            }

            return product;
        }
    }
}