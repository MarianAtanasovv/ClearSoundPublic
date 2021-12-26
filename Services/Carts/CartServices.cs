using ClearSoundCompany.Data;
using ClearSoundCompany.Data.Models.Cart;
using ClearSoundCompany.Models.Carts;
using ClearSoundCompany.Models.Products.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace ClearSoundCompany.Services.Carts
{
    public class CartServices
    {
        private readonly ClearSoundDbContext _data;

        public CartServices(ClearSoundDbContext data)
        {
            _data = data;
        }

        //Index page for showing all products inside the cart
        public TotalPriceDiscountAndListOfCartsViewModel Index(string userIdValue)
        {
            var newList = new List<CartViewModel>();

            var allCartProducts = AllCartProductsForUser(userIdValue);

            double totalPrice = 0;
            double paintAddition = 0;
            var numberOfProducts = 0;

            foreach (var cartProduct in allCartProducts)
            {
                var product = _data
                    .Products
                    .Include(i => i.Category)
                    .Include(i => i.ProductImages)
                    .FirstOrDefault(i => i.Id == cartProduct.ProductId);

                var colorCollection = new List<string>();

                var profilePictureColor = string.Empty;
                var selectedColor = string.Empty;

                if (product == null) continue;

                foreach (var image in product.ProductImages)
                {
                    var regexImage = Regex.Match(image.Name,
                        @"(?<Name>\w*)_(?<Color>\w*).(?<Index>\d*).(?<Extension>\w*)");

                    var regColor = regexImage.Groups["Color"].ToString().ToLower();

                    if (!Enum.TryParse(regColor, out Color color))
                    {
                        continue;
                    }

                    if (!colorCollection.Contains(regColor))
                    {
                        colorCollection.Add(regColor);
                    }

                    if (profilePictureColor == string.Empty && cartProduct.Color == color)
                    {
                        
                        profilePictureColor =$"{regexImage.Groups["Name"]}_{regexImage.Groups["Color"]}.7.{regexImage.Groups["Extension"]}";
                        selectedColor = regColor;
                    }
                }

                var newProduct = new CartViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Image = profilePictureColor,
                    Price = product.Price,
                    Category = product.Category.Name,
                    CategoryId = product.CategoryId,
                    Quantity = cartProduct.Quantity,
                    Colors = colorCollection,
                    SelectedColor = selectedColor
                };

                newList.Add(newProduct);

                totalPrice += newProduct.Price * newProduct.Quantity;

                if (cartProduct.MixedColor)
                {
                    paintAddition += CartConstant.PaintAudition * newProduct.Quantity;
                }

                numberOfProducts += newProduct.Quantity;
            }

            var discount = 0.00;
            var discountPercentage = 0.00;

            if (numberOfProducts > CartConstant.MinimumProductsQuantityForDiscount)
            {
                if (totalPrice >= CartConstant.MinimumPriceForDiscount)
                {
                    if (totalPrice / 1000 <= 45)
                    {
                        discount = totalPrice * (Math.Floor(totalPrice / 100000));
                        discountPercentage = Math.Floor(totalPrice / 1000);
                    }
                    else
                    {
                        discount = totalPrice * 0.45;
                        discountPercentage = 45;
                    }
                }
            }

            var model = new TotalPriceDiscountAndListOfCartsViewModel
            {
                Products = newList,
                DiscountPrice = Math.Round(discount),
                TotalPrice = Math.Round(totalPrice),
                DiscountPercentage = discountPercentage,
                PaintingAddition = paintAddition
            };

            return model;
        }

        public bool SubmitOrder(string userIdValue)
        {
            var cartProductsCollection = AllCartProductsForUser(userIdValue);

            var user = _data.Users.FirstOrDefault(i => i.Id == userIdValue);

            var emailNew = new EmailSender();

            StringBuilder emailStringBuilder = new();

            var discount = 0.00;
            var discountPercentage = 0.00;
            var totalPrice = 0.00;
            var paintAddition = 0.00;

            foreach (var cart in cartProductsCollection)
            {
                emailStringBuilder.AppendLine("<b>Product: </b>" + cart.Product.Name + "<br>");
                emailStringBuilder.AppendLine("<b>Color: </b>" + cart.Color + "<br>");
                emailStringBuilder.AppendLine("<b>Quantity: </b>" + cart.Quantity + "<br><br>");

                if (cart.MixedColor)
                {
                    paintAddition += CartConstant.PaintAudition * cart.Quantity;
                    totalPrice += (cart.Product.Price + CartConstant.PaintAudition) * cart.Quantity;
                }
                else
                {
                    totalPrice += cart.Product.Price * cart.Quantity;
                }

                //var newArchiveCart = new CartArchive{}
            }

            if (totalPrice >= CartConstant.MinimumPriceForDiscount)
            {
                if (totalPrice / 1000 <= 45)
                {
                    discount = totalPrice * (Math.Floor(totalPrice / 100000));
                    discountPercentage = Math.Floor(totalPrice / 1000);
                }
                else
                {
                    discount = totalPrice * 0.45;
                    discountPercentage = 45;
                }
            }

            var finalPrice = totalPrice - discount;

            emailStringBuilder.AppendLine("<b>Total price: </b>" + totalPrice + " EUR <br>");
            emailStringBuilder.AppendLine("<b>Discount: </b>" + discount + $"({discountPercentage}%) EUR<br>");
            emailStringBuilder.AppendLine("<b>Paint addition: </b>" + paintAddition + " EUR<br>");
            emailStringBuilder.AppendLine("<b>Final Price: </b>" + finalPrice + " EUR");

            var subject = "New Order";
            var messageNew = "<b>Email: </b>" + user.Email + "<br><b>Phone: </b>" + user.PhoneNumber + "<br><br>" +
                             emailStringBuilder.ToString();

            emailNew.SendEmailAsync("csc_audio@abv.bg", subject, messageNew);


            foreach (var cart in cartProductsCollection)
            {
                _data.Carts.Remove(cart);
            }

            _data.SaveChanges();

            return true;
        }

        public bool AddProductQuantity(string userIdValue, string productId)
        {
            var product = AllCartProductsForUser(userIdValue)
                .FirstOrDefault(i => i.ProductId == productId);

            if (product is {Quantity: < 50})
            {
                product.Quantity++;
            }
            else
            {
                return false;
            }

            _data.SaveChanges();
            return true;
        }

        public bool RemoveProductQuantity(string userIdValue, string productId)
        {
            var product = AllCartProductsForUser(userIdValue)
                .FirstOrDefault(i => i.ProductId == productId);

            var productData = _data.Products.FirstOrDefault(i => i.Id == productId);

            var categoryMinimumQuantity =
                _data.Categories.FirstOrDefault(i => i.Id == productData.CategoryId).MinimumQuantity;

            if (product != null && product.Quantity > categoryMinimumQuantity)
            {
                product.Quantity--;
            }
            else
            {
                return false;
            }

            _data.SaveChanges();
            return true;
        }

        //Delete all products from the cart
        public void ClearCart(string userIdValue)
        {
            var allCartProducts = _data.Carts.Where(i => i.UserId == userIdValue).ToList();
            foreach (var cart in allCartProducts)
            {
                _data.Carts.Remove(cart);
            }

            _data.SaveChanges();
        }

        //Delete one product from the cart
        [HttpGet]
        public void DeleteProduct(string userIdValue, string productId)
        {
            var product = _data
                .Carts
                .Where(i => i.UserId == userIdValue)
                .FirstOrDefault(i => i.ProductId == productId);

            if (product != null)
            {
                _data.Carts.Remove(product);
            }

            _data.SaveChanges();
        }

        //Add product to the cart
        public CartAddProductServicesViewModel AddProductCart(string userIdValue, string productId, bool different)
        {
            var product = _data.Products.Include(i => i.ProductImages).FirstOrDefault(i => i.Id == productId);
            var firstImage = product.ProductImages.FirstOrDefault();

            var regexImage = Regex.Match(firstImage.Name,
                @"(?<Name>\w*)_(?<Color>\w*).(?<Index>\d*).(?<Extension>\w*)");
            var regColor = regexImage.Groups["Color"].ToString().ToLower();

            var colorParse = Enum.TryParse(regColor, out Color color);

            var isItOkey = false;
            var message = "Operation failed";
            if (!colorParse)
            {
                return new CartAddProductServicesViewModel
                {
                    IsItOkey = false,
                    Message = message
                };
            }

            var ifProductExistInCart = _data
                .Carts
                .Where(i => i.UserId == userIdValue)
                .FirstOrDefault(i => i.ProductId == productId);

            if (different)
            {
                ifProductExistInCart = null;
            }
            if (ifProductExistInCart is {Quantity: < 50})
            {
                ifProductExistInCart.Quantity++;
                message =
                    $"Your product was added to your cart successfully! You have {ifProductExistInCart.Quantity} units into your shopping cart";
                isItOkey = true;
            }

            if (ifProductExistInCart == null)
            {
                var categoryQuantity =
                    _data.Categories.FirstOrDefault(i => i.Id == product.CategoryId).MinimumQuantity;

                var newCartProduct = new Cart()
                {
                    UserId = userIdValue,
                    Quantity = categoryQuantity,
                    ProductId = productId,
                    Color = color
                };
                _data.Carts.Add(newCartProduct);
                message =
                    $"Your product was added to your cart successfully! You have {newCartProduct.Quantity} units - that is the minimum quantity! ";
                isItOkey = true;
            }
            else if (ifProductExistInCart.Quantity >= 50)
            {
                message = "You can NOT add more than 50 units of one product!";
            }

            _data.SaveChanges();

            return new CartAddProductServicesViewModel
            {
                IsItOkey = isItOkey,
                Message = message
            };
        }

        //Change product color
        public void ColorChange(string userIdValue, string productId, string color)
        {
            var product = _data
                .Carts
                .Where(i => i.UserId == userIdValue)
                .FirstOrDefault(i => i.ProductId == productId);

            if (Enum.TryParse(color, out Color colorProductCart))
            {
                product.Color = colorProductCart;
                if (colorProductCart == Color.blackandwhite || colorProductCart == Color.blackandred)
                {
                    product.MixedColor = true;
                }
                else
                {
                    product.MixedColor = false;
                }
            }

            _data.SaveChanges();
        }

        public void AddProductCartDifferentColor(string userIdValue, string productId)
        {
            AddProductCart(userIdValue, productId, true);

            /*
            foreach (var cart in allSameModelsDifferentColors)
            {
                foreach (var image in product.ProductImages)
                {
                    var regexImage = Regex.Match(image.Name,
                        @"(?<Name>\w*)_(?<Color>\w*).(?<Index>\d*).(?<Extension>\w*)");

                    var regColor = regexImage.Groups["Color"].ToString().ToLower();

                    if (!Enum.TryParse(regColor, out Color color))
                    {
                        continue;
                    }

                    if (!colorCollection.Contains(regColor))
                    {
                        colorCollection.Add(regColor);
                    }

                    if (profilePictureColor == String.Empty && cart.Color == color)
                    {
                        profilePictureColor = image.Name;
                        selectedColor = regColor;
                    }
                }
            }
            */
        }

        private ICollection<Cart> AllCartProductsForUser(string userIdValue)
        {
            var allCartProductsForGivenUser = _data
                .Carts
                .Where(i => i.UserId == userIdValue)
                .ToList();

            return allCartProductsForGivenUser;
        }
    }
}