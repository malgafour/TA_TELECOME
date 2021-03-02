using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TA_TELECOME.Models;

namespace TA_TELECOME.Controllers
{
    public class AuthController : Controller
    {
        private DB db = new DB();

        // GET: Auth
        public ActionResult Login()
        {
            return View();
        }

        // POST: Auth
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var successlogin = db.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
                if (successlogin != null)
                {
                    if(successlogin.Active == false)
                    {
                        ViewBag.errorAuthantication = "<p> your not activated ... </p>";
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(successlogin.Id.ToString(), false);
                        return RedirectToAction("Index", "Home");
                    }

                    
                }

                else
                {
                    ViewBag.errorAuthantication = "<p> Username or password incorrect , please try again ... </p>";
                } 
            }
            return View();

        }
         
        [HttpPost]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}