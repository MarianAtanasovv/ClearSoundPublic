using ClearSoundCompany.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ClearSoundCompany.Areas.Administration.Models.Users;
using ClearSoundCompany.Services.Carts;

namespace ClearSoundCompany.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly ClearSoundDbContext _data;
        private readonly CartServices _cartServices;

        public UsersController(ClearSoundDbContext data, CartServices cartServices)
        {
            _cartServices = cartServices;
            _data = data;
        }

        public IActionResult Index()
        {
            var allUsers = _data.Users;
            var collectionUsers = new List<UserViewModel>();

            if (_data.UserRoles == null) return View(collectionUsers);

            var adminId = _data.UserRoles.FirstOrDefault().UserId;
            foreach (var user in allUsers)
            {
                if (user.Id == adminId)
                {
                    continue;
                }

                var newUser = new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    AccessFailedCount = user.AccessFailedCount,
                    EmailConfirmed = user.EmailConfirmed,
                    CanYouLockIt = user.LockoutEnabled
                };
                if (user.LockoutEnd > DateTime.Now)
                {
                    newUser.LockoutDate = user.LockoutEnd.Value.DateTime.ToShortDateString();
                    newUser.LockoutTime = user.LockoutEnd.Value.DateTime.ToShortTimeString();
                }

                newUser.Phone = user.PhoneNumber;
                collectionUsers.Add(newUser);
            }

            return View(collectionUsers);
        }

        public IActionResult Orders(string id)
        {
            var model = _cartServices.Archive(id);
            return View("~/Views/Carts/Archive.cshtml", model);
        }
        public IActionResult Cart(string id)
        {
            var model = _cartServices.Index(id);
            return View("~/Views/Carts/Index.cshtml", model);
        }
        public IActionResult LockAccount(string email)
        {
            var user = _data.Users.FirstOrDefault(i => i.Email == email);
            if (user != null) user.LockoutEnd = DateTime.MaxValue;
            _data.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult UnlockAccount(string email)
        {
            var user = _data.Users.FirstOrDefault(i => i.Email == email);
            if (user != null) user.LockoutEnd = null;
            _data.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ActivateEmail(string email)
        {
            var user = _data.Users.FirstOrDefault(i => i.Email == email);
            if (user != null) user.EmailConfirmed = true;
            _data.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeactivateEmail(string email)
        {
            var user = _data.Users.FirstOrDefault(i => i.Email == email);
            if (user != null) user.EmailConfirmed = false;
            _data.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ActivateLocking(string email)
        {
            var user = _data.Users.FirstOrDefault(i => i.Email == email);
            if (user != null) user.LockoutEnabled = true;
            _data.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeactivateLocking(string email)
        {
            var user = _data.Users.FirstOrDefault(i => i.Email == email);
            if (user != null) user.LockoutEnabled = false;
            _data.SaveChanges();
            return RedirectToAction("Index");
        }

        public ICollection<UserViewModel> AllUsers()
        {
            var allUsers = _data.Users;
            var collectionUsers = new List<UserViewModel>();
            foreach (var user in allUsers)
            {
                var newUser = new UserViewModel
                {
                    Email = user.Email,
                    AccessFailedCount = user.AccessFailedCount,
                    EmailConfirmed = user.EmailConfirmed,
                    CanYouLockIt = user.LockoutEnabled,
                    Phone = user.PhoneNumber
                };
                collectionUsers.Add(newUser);
            }

            return collectionUsers;
        }
    }
}