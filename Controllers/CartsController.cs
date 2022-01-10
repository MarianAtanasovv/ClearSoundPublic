using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using ClearSoundCompany.Services.Carts;

namespace ClearSoundCompany.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly CartServices _cartServices;

        public CartsController(CartServices cartServices)
        {
            _cartServices = cartServices;
        }
        
        public IActionResult Index()
        {
            var userIdValue = GetUserId();

            if (userIdValue == null) return View();

            var model = _cartServices.Index(userIdValue);

            return View(model);
        }

        public IActionResult Archive()
        {
            var userIdValue = GetUserId();

            if (userIdValue == null) return View();
            var model = _cartServices.Archive(userIdValue);

            return View(model);
        }

        public IActionResult SubmitOrder()
        {
            var userIdValue = GetUserId();
            if (userIdValue == null) return RedirectToAction("Index", "Carts");


            try
            {
                _cartServices.SubmitOrder(userIdValue);
                TempData["alertSuccess"] =
                    "Your order was submitted! Please wait a few days for it to be processed - the proforma will be send to your email address!";
            }
            catch (Exception exception)
            {
                
                TempData["alertDanger"] =
                    $"There is a problem... Please try again later{exception}";
            }

            return RedirectToAction("Index", "Carts");
        }

        [HttpGet]
        public IActionResult AddProductQuantity(string productId)
        {
            var userIdValue = GetUserId();

            if (userIdValue == null) return RedirectToAction("Index", "Carts");

            if (!_cartServices.AddProductQuantity(userIdValue, productId))
            {
                TempData["alertWarning"] = "Maximum quantity limit is reached!";
            }

            return RedirectToAction("Index", "Carts");
        }

        public JsonResult AddProductQuantityJsonResult(string productId)
        {
            var isItOkey = false;
            var message = "Operation failed";

            var userIdValue = GetUserId();

            if (userIdValue == null) return Json(new {success = isItOkey, msg = message});

            if (!_cartServices.AddProductQuantity(userIdValue, productId))
            {
                message = "Maximum quantity limit is reached!";
                return Json(new {success = isItOkey, msg = message});
            }

            var model = _cartServices.Index(userIdValue);
            isItOkey = true;
            // return RedirectToAction("Index", "Carts");

            return Json(new {success = isItOkey, msg = model});
        }

        public JsonResult RemoveProductQuantityJsonResult(string productId)
        {
            var isItOkey = false;
            var message = "Operation failed";

            var userIdValue = GetUserId();

            if (userIdValue == null) return Json(new {success = isItOkey, msg = message});

            if (!_cartServices.RemoveProductQuantity(userIdValue, productId))
            {
                message = "Minimum quantity limit is reached!";
                return Json(new {success = isItOkey, msg = message});
            }

            var model = _cartServices.Index(userIdValue);
            isItOkey = true;
            // return RedirectToAction("Index", "Carts");

            return Json(new {success = isItOkey, msg = model});
        }

        [HttpGet]
        public IActionResult RemoveProductQuantity(string productId)
        {
            var userIdValue = GetUserId();
            if (userIdValue == null) return RedirectToAction("Index", "Carts");

            if (!_cartServices.RemoveProductQuantity(userIdValue, productId))
            {
                TempData["alertWarning"] = "Minimum quantity limit is reached!";
            }

            return RedirectToAction("Index", "Carts");
        }

        //Delete all products from the cart
        public IActionResult ClearCart()
        {
            var userIdValue = GetUserId();
            if (userIdValue == null) return RedirectToAction("Index", "Carts");

            _cartServices.ClearCart(userIdValue);

            return RedirectToAction("AllCategories", "Products");
        }

        //Delete one product from the cart
        [HttpGet]
        public IActionResult DeleteProduct(string productId)
        {
            var userIdValue = GetUserId();
            if (userIdValue != null)
            {
                _cartServices.DeleteProduct(userIdValue, productId);
            }

            return RedirectToAction("Index", "Carts");
        }

        //Add product to the cart
        [HttpPost]
        public JsonResult AddProductCart(string productId)
        {
            var isItOkey = false;
            var message = "Operation failed";

            var userIdValue = GetUserId();
            if (userIdValue != null)
            {
                var result = _cartServices.AddProductCart(userIdValue, productId, false);
                isItOkey = result.IsItOkey;
                message = result.Message;
            }

            return Json(new {success = isItOkey, msg = message});
        }

        /* public IActionResult AddProductCartDifferentColor(string productId)
         {
             var userIdValue = GetUserId();
             if (userIdValue != null)
             {
                 _cartServices.AddProductCartDifferentColor(userIdValue, productId);
             }
 
             return RedirectToAction("Index", "Carts");
         }
        */

        //Change product color
        [HttpGet]
        public IActionResult ColorChange(string productId, string color)
        {
            var userIdValue = GetUserId();
            if (userIdValue != null)
            {
                _cartServices.ColorChange(userIdValue, productId, color);
            }

            return RedirectToAction("Index", "Carts");
        }

        //Getting User Id. Returns Null if somethings is wrong :P 
        private string GetUserId()
        {
            string userIdValue = null;
            if (User.Identity is ClaimsIdentity claimsIdentity)
            {
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    userIdValue = userIdClaim.Value;
                }
            }

            return userIdValue;
        }
    }
}