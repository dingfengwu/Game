﻿using Microsoft.AspNetCore.Http;

namespace Game.Services.Authentication
{
    /// <summary>
    /// Default values related to cookie-based authentication handler
    /// </summary>
    public static class GameCookieAuthenticationDefaults
    {
        /// <summary>
        /// The default value used for authentication scheme
        /// </summary>
        public const string AuthenticationScheme = "Authentication";

        /// <summary>
        /// The default value used for external authentication scheme
        /// </summary>
        public const string ExternalAuthenticationScheme = "ExternalAuthentication";

        /// <summary>
        /// The prefix used to provide a default cookie name
        /// </summary>
        public static readonly string CookiePrefix = ".Game.";

        /// <summary>
        /// The issuer that should be used for any claims that are created
        /// </summary>
        public static readonly string ClaimsIssuer = "kkgame";

        /// <summary>
        /// The default value for the login path
        /// </summary>
        public static readonly PathString LoginPath = new PathString("/admin/account/signin");

        /// <summary>
        /// The default value used for the logout path
        /// </summary>
        public static readonly PathString LogoutPath = new PathString("/admin/account/signout");

        /// <summary>
        /// The default value for the access denied path
        /// </summary>
        public static readonly PathString AccessDeniedPath = new PathString("/page-not-found");

        /// <summary>
        /// The default value of the return URL parameter
        /// </summary>
        public static readonly string ReturnUrlParameter = "";
    }
}
