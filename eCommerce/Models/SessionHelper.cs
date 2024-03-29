﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models
{
    /// <summary>
    /// Helper class to provide Session
    /// management
    /// </summary>
    public static class SessionHelper
    {

        private const string MemberIdKey = "memberId";
        private const string UsernameKey = "username";


        public static void LogUserIn(IHttpContextAccessor context
                                    , int memberId
                                    , string username)
        {
            context.HttpContext.Session.SetInt32(MemberIdKey, memberId);
            context.HttpContext.Session.SetString(UsernameKey, username);


        }


        public static bool IsUserLoggedIn(IHttpContextAccessor context)
        {
            if (context.HttpContext.Session.GetInt32(MemberIdKey).HasValue)
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// Destroys current users session
        /// </summary>
        /// <param name="context"></param>
        public static void LogUserOut(IHttpContextAccessor context)
        {
            context.HttpContext.Session.Clear();
        }


        /// <summary>
        /// Gets the username of the current user if they are logged in. Null 
        /// is returned otherwise.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetUsername(IHttpContextAccessor context)
        {
            return context.HttpContext.Session.GetString(UsernameKey);
        }



        /// <summary>
        /// Returns the MemberId of the currently logged in user. MemberId
        /// will be null if not logged in.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int? GetMemberId(IHttpContextAccessor context)
        {
            return context.HttpContext.Session.GetInt32(MemberIdKey);
        }




    }
}
