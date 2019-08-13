using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Data;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    public class AccountController : Controller
    {
        // added constructor to access database, the context datbase
        private readonly GameContext _context; 

        public AccountController(GameContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Register(Member m)
        {
            if (ModelState.IsValid)
            {
                await MemberDb.Add(_context, m);

                TempData["Message"] = "You registered successfully";
                return RedirectToAction("Index", "Home");

            }
            return View(m);
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isMember = await MemberDb.IsLoginValid(model, _context);
                if (isMember)
                {
                    TempData["Message"] = "Logged in successfully";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // credentials invalid but password and username is supplied
                    // AddModelError without key displays error message in the validation summary that is auto generated and for modelErrors
                    ModelState.AddModelError(string.Empty,"I'm sorry, you're credentials did not match any records in our database");
                }
            }

            return View(model);
        }


        


    }
}