using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Data;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    public class LibraryController : Controller
    {
        // can't reassign _context
        private readonly GameContext _context;


        public LibraryController(GameContext context)
        {
            context = _context;
        }

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
                _context.Add(game);
                _context.SaveChanges();


                return RedirectToAction("Index");

            }

            // if not valid: return view with model including error messages
            return View(game);

        }


    }
}