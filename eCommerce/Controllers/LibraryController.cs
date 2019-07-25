using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    public class LibraryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // get request for library/add, simply returns view
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(VideoGame game)
        {
            // model binding, if all data annotations validate
            if (ModelState.IsValid)
            {
                // add to database
                return RedirectToAction("Index");

            }

            // if not valid: return view with model including error messages
            return View(game);

        }


    }
}