using AspNetCore.ReCaptcha;
using ClearSoundCompany.Models.Contacts;
using ClearSoundCompany.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClearSoundCompany.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ReCaptcha _captcha;

        public ContactsController(ReCaptcha captcha)
        {
            _captcha = captcha;
        }

        public IActionResult Send()
        {
            return View();
        }

        [ValidateReCaptcha]
        [HttpPost]
        public IActionResult Send(SendEmailFormModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["alertWarning"] = "Please try again!";
                return View();
            }

            var emailNew = new EmailSender();
            var messageNew = "<b>Email: </b>" + model.Email + "<br><b> Name: </b>" + model.Name + "<br><br>" + model.Message;
            emailNew.SendEmailAsync("csc_audio@abv.bg", model.Subject, messageNew);
            TempData["alertSuccess"] = "Your message was sent successfully!";

            return RedirectToAction("Send", "Contacts");
        }
    }
}