using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    public class AccountController : Controller
    {

        // add constructor with DB Context as a parameter


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }




        [HttpPost]
        public IActionResult Register(Member m)
        {
            // TODO: Add member to database
            // Display success message on home page after redirecting


        }


    }
}