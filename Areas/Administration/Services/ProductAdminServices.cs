using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClearSoundCompany.Areas.Administration.Models.Products;
using ClearSoundCompany.Data;
using ClearSoundCompany.Data.Models.Products;
using ClearSoundCompany.Models.Products;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace ClearSoundCompany.Areas.Administration.Services
{
    public class ProductAdminServices
    {
        private readonly ClearSoundDbContext _data;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductAdminServices(ClearSoundDbContext data, IWebHostEnvironment hostEnvironment)
        {
            _data = data;
            _webHostEnvironment = hostEnvironment;
        }

        //Post method for adding a Product to the database
        public void AddProduct(AddProductFormModel product)
        {
            //If everything is okey, all file names are renamed with some random name

            var uploadingFile = new UploadingFile(_webHostEnvironment);
            var uniqueProductDataName = uploadingFile.UploadFileAndChangeName(product.ProductData, "Products");
            var uniqueProductProfileImage = uploadingFile.UploadFileAndChangeName(product.ProfileImage, "Products");
            var uniqueProductCompressedProfileImage =
                uploadingFile.UploadFileAndChangeName(product.CompressedProfileImage, "Products");
            var uniqueEASEDataName = uploadingFile.UploadFileAndChangeName(product.EASEData, "Products");
            var uniqueManualName = uploadingFile.UploadFileAndChangeName(product.Manual, "Products");

            var productData = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                ProfileImage = uniqueProductProfileImage,
                CompressedProfileImage = uniqueProductCompressedProfileImage,
                ProductData = uniqueProductDataName,
                EaseData = uniqueEASEDataName,
                Manual = uniqueManualName,
                New = product.New
            };

            foreach (var image in product.Images)
            {
                var uniqueFileName = uploadingFile.UploadFile(image, "Products");

                var productImageData = new ProductImage
                {
                    Name = uniqueFileName,
                    ProductId = productData.Id
                };
                _data.ProductImages.Add(productImageData);
                productData.ProductImages.Add(productImageData);
            }

            //Going through all specifications from the Form and adding them to the specific Product collection
            var order = 0;
            foreach (var specificationInput in product.Specifications)
            {
                if (specificationInput == null) continue;

                var specification = new Specification
                {
                    Name = specificationInput.SpecificationName,
                    Description = specificationInput.SpecificationDescription,
                    Order = order
                };
                productData.Specifications.Add(specification);
                order++;
            }
            //Adding to the database

            _data.Products.Add(productData);
            _data.SaveChanges();
        }

        //View for Editing a specific Product
        public EditProductFormModel PreviewAllInformationForEditing(string id)
        {
            var product = _data
                .Products
                .Include(i => i.Specifications)
                .FirstOrDefault(i => i.Id == id);

            if (product == null) return null;

            //Taking all Product Specification and sending them to the View

            var specifications = new List<ProductSpecificationViewModel>();
            foreach (var specification in product.Specifications.OrderBy(i=>i.Order))
            {
                ProductSpecificationViewModel newSpecification = new()
                {
                    SpecificationName = specification.Name,
                    SpecificationDescription = specification.Description
                };
                specifications.Add(newSpecification);
            }

            return (new EditProductFormModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Categories = GetProductCategories(),
                Specifications = specifications,
                New = product.New
            });
        }

        //Post method for Editing a Product
        public void EditProduct(EditProductFormModel product)
        {
            var uploadingFile = new UploadingFile(_webHostEnvironment);

            var uniqueProductDataName = uploadingFile.UploadFileAndChangeName(product.ProductData, "Products");
            var uniqueProductProfileImage = uploadingFile.UploadFileAndChangeName(product.ProfileImage, "Products");
            var uniqueProductCompressedProfileImage =
                uploadingFile.UploadFileAndChangeName(product.CompressedProfileImage, "Products");
            var uniqueEASEDataName = uploadingFile.UploadFileAndChangeName(product.EASEData, "Products");
            var uniqueManualName = uploadingFile.UploadFileAndChangeName(product.Manual, "Products");

            var existingProduct = _data.Products
                .Include(i => i.Specifications)
                .FirstOrDefault(i => i.Id == product.Id);

            if (existingProduct == null)
            {
                return;
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.New = product.New;

            if (product.Images != null)
            {
                foreach (var image in product.Images)
                {
                    var isThereTheSameImage = _data
                        .ProductImages
                        .Any(i => i.Name == image.FileName);

                    if (isThereTheSameImage)
                    {
                        var oldImage = _data.ProductImages.FirstOrDefault(i => i.Name == image.FileName);
                        if (oldImage != null)
                        {
                            _data.ProductImages.Remove(oldImage);
                            string path = Path.Combine(_webHostEnvironment.WebRootPath, "database_files/Products/") +
                                          oldImage.Name;
                            FileInfo file = new(path);
                            if (file.Exists)
                            {
                                file.Delete();
                            }

                            _data.SaveChanges();
                        }
                    }

                    var uniqueFileName = uploadingFile.UploadFile(image, "Products");

                    var productImageData = new ProductImage
                    {
                        Name = uniqueFileName,
                        ProductId = product.Id
                    };
                    _data.ProductImages.Add(productImageData);
                    existingProduct.ProductImages.Add(productImageData);
                }
            }

            if (uniqueProductDataName != null) existingProduct.ProductData = uniqueProductDataName;
            if (uniqueProductProfileImage != null) existingProduct.ProfileImage = uniqueProductProfileImage;
            if (uniqueProductCompressedProfileImage != null)
                existingProduct.CompressedProfileImage = uniqueProductCompressedProfileImage;
            if (uniqueEASEDataName != null) existingProduct.EaseData = uniqueEASEDataName;
            if (uniqueManualName != null) existingProduct.Manual = uniqueManualName;

            existingProduct.Specifications.Clear();
            //Going through all specifications from the Form and adding them to the specific Product collection
            var order = 0;
            foreach (var specificationInput in product.Specifications)
            {
                if (specificationInput == null) continue;

                var specification = new Specification
                {
                    Name = specificationInput.SpecificationName,
                    Description = specificationInput.SpecificationDescription,
                    Order = order
                };
                existingProduct.Specifications.Add(specification);
                order++;
            }

            //Saving the database
            _data.SaveChanges();
        }

        //View for Deleting image
        public Product AllImagesForProduct(string id)
        {
            var images = _data
                .Products
                .Include(i => i.ProductImages)
                .FirstOrDefault(i => i.Id == id);
            return images;
        }

        //View for all Categories (for editing)
        public List<Category> AllCategoriesForEditAndDelete()
        {
            var products = _data
                .Categories
                .OrderBy(i => i.Name)
                .Include(i => i.Products)
                .ToList();

            return products;
        }

        //Post method for Adding a Category
        public void AddCategory(AddCategoryFormModel category)
        {
            var categoryData = new Category
            {
                Name = category.Name,
                MinimumQuantity = category.MinimumQuantity,
                IsAddable = category.IsAddable
            };
            _data.Categories.Add(categoryData);
            _data.SaveChanges();
        }

        //View for Editing specific Category
        public EditCategoryFormModel EditCategories(string id)
        {
            var category = _data.Categories.FirstOrDefault(i => i.Id == id);

            if (category != null)
                return new EditCategoryFormModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsAddable = category.IsAddable,
                    MinimumQuantity = category.MinimumQuantity
                };
            return null;
        }

        //Post method for Editing specific Category
        public void EditCategories(EditCategoryFormModel category)
        {
            var categoryData = _data.Categories.FirstOrDefault(i => i.Id == category.Id);
            if (categoryData == null) return;

            categoryData.Name = category.Name;
            categoryData.IsAddable = category.IsAddable;
            categoryData.MinimumQuantity = category.MinimumQuantity;
            _data.SaveChanges();
        }

        //Cascade deleting a Product
        public void DeleteProduct(string id)
        {
            var product = _data
                .Products
                .Include(i => i.Specifications)
                .Include(i => i.ProductImages)
                .FirstOrDefault(i => i.Id == id);
            if (product == null) return;

            var images = product.ProductImages.ToList();

            foreach (var image in images)
            {
                //Build the File Path.
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "database_files/Products/") +
                              image.Name;
                FileInfo file = new(path);

                if (file.Exists)
                {
                    file.Delete();
                }
            }

            var pathProfileImage =
                Path.Combine(_webHostEnvironment.WebRootPath, "database_files/Products/") +
                product.ProfileImage;

            FileInfo fileProfileImage = new(pathProfileImage);

            if (fileProfileImage.Exists)
            {
                fileProfileImage.Delete();
            }

            _data.Products.Remove(product);
            _data.SaveChanges();
        }

        //Cascade deleting an Image
        public void DeleteImage(string id)
        {
            var image = _data
                .ProductImages
                .FirstOrDefault(i => i.Id == id);

            if (image == null) return;

            DeletingImage(image);

            _data.SaveChanges();
        }


        //Cascade deleting all Images
        public string DeleteAllImages(string id)
        {
            var product = _data
                .Products
                .Include(i => i.ProductImages)
                .FirstOrDefault(i => i.Id == id);

            if (product != null)
            {
                foreach (var image in product.ProductImages)
                {
                    DeletingImage(image);
                }

                _data.SaveChanges();
                return product.Id;
            }

            return null;
        }

        //Cascade deleting a Category
        public void DeleteCategory(string id)
        {
            var category = _data
                .Categories
                .Include(i => i.Products)
                .FirstOrDefault(i => i.Id == id);

            if (category == null || category.Products.Count != 0) return;

            _data.Categories.Remove(category);
            _data.SaveChanges();
        }

        //----------------Methods------------------
        //Method for returning all Product Categories from the database
        private void DeletingImage(ProductImage image)
        {
            _data.ProductImages.Remove(image);
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "database_files/Products/") +
                       image.Name;
            FileInfo file = new(path);
            if (file.Exists)
            {
                file.Delete();
            }
        }

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