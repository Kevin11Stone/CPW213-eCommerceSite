using eCommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Data
{

    /// <summary>
    /// DB helper class for VideoGames
    /// </summary>
    public static class VideoGameDb
    {


        /// <summary>
        /// Returns one page worth of products. Products are sorted alphabetically by title.
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="pageNum">The page number of the products you want</param>
        /// <param name="pageSize">The number of products per page</param>
        /// <returns></returns>
         public static async Task<List<VideoGame>> GetGamesByPage(GameContext context, int pageNum, int pageSize)
         {

            // make sure to call skip Before take
            // make sure orderBy comes first
            List<VideoGame> games = await context.VideoGames
                                                 .OrderBy(vg => vg.Title)
                                                 .Skip( (pageNum - 1) * pageSize )
                                                 .Take(pageSize)                                               
                                                 .ToListAsync();
            return games;
         }



        /// <summary>
        /// Returns the total number of pages needed to have <paramref name="pageSize"/> amount of products per page
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<int> GetTotalPages(GameContext context, int pageSize)
        {
            int totalNumGames = await context.VideoGames.CountAsync();

            double pages = (double)totalNumGames / pageSize;

            // convert back to int with cast, had to cast to double due to rounding issues, ceiling rounds up
            return (int)Math.Ceiling(pages);
        }





        // had to add using

        /// <summary>
        /// Adds a VideoGame to the data store and sets the ID value
        /// </summary>
        /// <param name="g">The game to add</param>
        /// <param name="context">The database context to use</param>
        /// <returns>VideoGame with ID populated</returns>
        public static async Task<VideoGame> Add(VideoGame g, GameContext context)
        {
            await context.AddAsync(g);
            await context.SaveChangesAsync();
            return g;
        }




        /// <summary>
        /// Retrieves all games sorted in alphabetical order by
        /// title
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<List<VideoGame>> GetAllGames(GameContext context)
        {
            // LINQ Query syntax
            //List<VideoGame> games = await (from vidGame in context.VideoGames
            //                         orderby vidGame.Title ascending
            //                         select vidGame).ToListAsync();
            List<VideoGame> games = await context.VideoGames.
                                        OrderBy(g => g.Title)
                                        .ToListAsync();
            return games;
        }



        /// <summary>
        /// Updates a single video game
        /// </summary>
        /// <param name="g"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<VideoGame> UpdateGame(VideoGame g, GameContext context)
        {
            context.Update(g);
            await context.SaveChangesAsync();
            return g;
        }




        /// <summary>
        /// Gets a game with a specified Id. If not game is found,
        /// null is returned.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<VideoGame> GetGameById(int id, GameContext context)
        {
            VideoGame g = await (from game in context.VideoGames
                           where game.Id == id
                           select game).SingleOrDefaultAsync();
            return g;
        }






        public static async Task DeleteById(int id, GameContext context)
        {
            // create video game with the id we want to remove
            VideoGame g = new VideoGame()
            {
                Id = id
            };

            // tell context we want to remove this game from the database, then save
            context.Entry(g).State = EntityState.Deleted;
            await context.SaveChangesAsync();

        }

        /// <summary>
        /// Searches for games that match the criteria 
        /// and returns all games that match.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static async Task<List<VideoGame>> Search(GameContext context, SearchCriteria criteria)
        {
            // SELECT * FROM VideoGames - this DOES NOT Query the database
            IQueryable<VideoGame> allGames = from g in context.VideoGames
                                             select g;
            if (criteria.MinPrice.HasValue)
            {
                // adds to where clause: game.Price >= criteria.MinPrice
                allGames = from g in allGames
                           where g.Price >= criteria.MinPrice
                           select g;
            }
            if (criteria.MaxPrice.HasValue)
            {
                allGames = from g in allGames
                           where g.Price <= criteria.MaxPrice
                           select g;
            }
            if (!string.IsNullOrWhiteSpace(criteria.Title))
            {
                // WHERE Game.Title startsWith criteria.Title
                allGames = from g in allGames
                           where g.Title.StartsWith(criteria.Title)
                           select g;
            }
            if (!string.IsNullOrWhiteSpace(criteria.Rating))
            {
                // WHERE Game.Rating = critera.Rating
                allGames = from g in allGames
                           where g.Rating == criteria.Rating
                           select g;
            }

            // send final query to database to return results
            // EF does not send the query until it has to through methods ( ToList(), Contains(), etc. )
            return await allGames.ToListAsync();

        }
    }


}
