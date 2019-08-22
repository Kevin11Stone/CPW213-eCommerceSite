using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models
{

    /// <summary>
    /// Contains helper methods to manage the users shopping cart
    /// </summary>
    public static class CartHelper
    {
        private const string CartCookie = "Cart";


        /// <summary>
        /// Gets current user's videoGames from their shopping
        /// cart. If no games, an empty list is returned.
        /// </summary>
        /// <param name="accessor"></param>
        /// <returns></returns>
        public static List<VideoGame> GetGames(IHttpContextAccessor accessor)
        {
            // get data out of cookie
            string data = accessor.HttpContext.Request.Cookies[CartCookie];

            if (string.IsNullOrWhiteSpace(data))
            {
                return new List<VideoGame>();
            }

            // Json.NET
            List<VideoGame> games = JsonConvert.DeserializeObject<List<VideoGame>>(data);

            return games;
        }

        /// <summary>
        /// Gets total number of videoGames in the cart
        /// </summary>
        /// <param name="accessor"></param>
        /// <returns></returns>
        public static int GetGameCount(IHttpContextAccessor accessor)
        {
            List<VideoGame> allGames = GetGames(accessor);
            return allGames.Count;
        }


        /// <summary>
        /// Adds videoGame to the existing cart data.
        /// If no cart cookie exists, it will be created.
        /// (Adds videoGame to cart)
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="g">The game to be added</param>
        public static void Add(IHttpContextAccessor accessor, VideoGame g)
        {
            List<VideoGame> games = GetGames(accessor);
            games.Add(g);

            string data = JsonConvert.SerializeObject(games);

            // overide cookie data
            accessor.HttpContext.Response.Cookies.Append(CartCookie, data);
        }



    }
}
