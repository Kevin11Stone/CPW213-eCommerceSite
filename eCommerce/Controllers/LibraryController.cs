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
            _context = context;
        }

        // only need HttpGet in this case: we want to make this URL bookmarkable
        [HttpGet]
        public async Task<IActionResult> Search(SearchCriteria criteria)
        {
            if (ValidSearch(criteria))
            {
                criteria.GameResults = await VideoGameDb.Search(_context, criteria);
            }
            //else
            //{
            //    // handle NullException()
            //    criteria.GameResults = new List<VideoGame>();
            //}
            
            // returns view with the given information
            return View(criteria);
        }

        /// <summary>
        /// Returns true if user searched by at least one
        /// piece of criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        private bool ValidSearch(SearchCriteria criteria)
        {
            if (criteria.Title == null &&
                criteria.Rating == null &&
                criteria.MinPrice == null &&
                criteria.MaxPrice == null)
            {
                return false;
            }
            return true;
        }

        [HttpGet] // made id optional using ?
        public async Task<IActionResult> Index(int? id)
        {
            // id is the page number coming in
            // null-coalescing operator = if id is not null, set page to it. if null, use 1
            int page = id ?? 1;
            const int PageSize = 3;
            List<VideoGame> games = await VideoGameDb.GetGamesByPage(_context, page, PageSize);


            int totalPages = await VideoGameDb.GetTotalPages(_context, PageSize);

            ViewData["Pages"] = totalPages;
            ViewData["CurrentPage"] = page;
            // the view as access to all of the data
            return View(games);
        }



        // get request for library/add, simply returns view
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Add(VideoGame game)
        {
            // model binding, if all data annotations validate
            if (ModelState.IsValid)
            {
                // add to database
                await VideoGameDb.Add(game, _context);


                return RedirectToAction("Index");

            }

            // if not valid: return view with model including error messages
            return View(game);

        }




        public async Task<IActionResult> Update(int id)
        {
            VideoGame game = await VideoGameDb.GetGameById(id, _context);

            return View(game);
        }



        // runs when you fill out the form and submit it, send it to the server
        [HttpPost]
        public async Task<IActionResult> Update(VideoGame g)
        {
            if (ModelState.IsValid)
            {
                await VideoGameDb.UpdateGame(g, _context);
                return RedirectToAction("Index");
            }

            // returns same view with error messages
            return View(g);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            VideoGame game = await VideoGameDb.GetGameById(id, _context);
            return View(game);
        }
        


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await VideoGameDb.DeleteById(id, _context);
            return RedirectToAction("Index");
        }



    }
}