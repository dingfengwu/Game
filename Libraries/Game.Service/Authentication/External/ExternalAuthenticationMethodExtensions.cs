using System;
using Game.Base.Domain.Customers;

namespace Game.Services.Authentication.External
{
    /// <summary>
    /// Extensions of external authentication method 
    /// </summary>
    public static class ExternalAuthenticationMethodExtensions
    {
        /// <summary>
        /// Check whether external authentication method is active
        /// </summary>
        /// <param name="method">External authentication method</param>
        /// <param name="settings">External authentication settings</param>
        /// <returns>True if method is active; otherwise false</returns>
        public static bool IsMethodActive(this IExternalAuthenticationMethod method, ExternalAuthenticationSettings settings)
        {
            return false;
        }
    }
}
