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


      





    }


}
