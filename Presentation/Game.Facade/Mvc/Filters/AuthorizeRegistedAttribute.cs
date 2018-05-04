using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Game.Base.Data;
using Game.Services.Security;
using Game.Base;
using Game.Base.Domain.Customers;

namespace Game.Facade.Mvc.Filters
{
    /// <summary>
    /// Represents a filter attribute that confirms access to the admin panel
    /// </summary>
    public class AuthorizeRegistedAttribute : TypeFilterAttribute
    {
        #region Fields

        private readonly bool _ignoreFilter;

        #endregion

        #region Ctor

        /// <summary>
        /// Create instance of the filter attribute
        /// </summary>
        /// <param name="ignore">Whether to ignore the execution of filter actions</param>
        public AuthorizeRegistedAttribute(bool ignore = false) : base(typeof(AuthorizeRegistedFilter))
        {
            this._ignoreFilter = ignore;
            this.Arguments = new object[] { ignore };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether to ignore the execution of filter actions
        /// </summary>
        public bool IgnoreFilter => _ignoreFilter;

        #endregion


        #region Nested filter

        /// <summary>
        /// Represents a filter that confirms access to the admin panel
        /// </summary>
        private class AuthorizeRegistedFilter : IAuthorizationFilter
        {
            #region Fields

            private readonly bool _ignoreFilter;
            private readonly IWorkContext _workContext;

            #endregion

            #region Ctor

            public AuthorizeRegistedFilter(bool ignoreFilter, IWorkContext workContext)
            {
                this._ignoreFilter = ignoreFilter;
                this._workContext = workContext;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Called early in the filter pipeline to confirm request is authorized
            /// </summary>
            /// <param name="filterContext">Authorization filter context</param>
            public void OnAuthorization(AuthorizationFilterContext filterContext)
            {
                if (filterContext == null)
                    throw new ArgumentNullException(nameof(filterContext));

                //check whether this filter has been overridden for the action
                var actionFilter = filterContext.ActionDescriptor.FilterDescriptors
                    .Where(filterDescriptor => filterDescriptor.Scope == FilterScope.Action)
                    .Select(filterDescriptor => filterDescriptor.Filter).OfType<AuthorizeAdminAttribute>().FirstOrDefault();

                //ignore filter (the action is available even if a customer hasn't access to the admin area)
                if (actionFilter?.IgnoreFilter ?? _ignoreFilter)
                    return;

                if (!DataSettingsHelper.DatabaseIsInstalled())
                    return;

                //there is AdminAuthorizeFilter, so check access
                if (filterContext.Filters.Any(filter => filter is AuthorizeRegistedFilter))
                {
                    //authorize permission of access to the admin area
                    if (!_workContext.CurrentCustomer.IsRegistered())
                        filterContext.Result = new ChallengeResult();
                }
            }

            #endregion
        }

        #endregion
    }
}