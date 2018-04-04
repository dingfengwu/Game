﻿using Microsoft.AspNetCore.Routing;
using Game.Services.Seo;

namespace Game.Facade.Seo
{
    /// <summary>
    /// Represents event to handle unknown URL record entity names
    /// </summary>
    public class CustomUrlRecordEntityNameRequested
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="routeData">Route data</param>
        /// <param name="urlRecord">URL record</param>
        public CustomUrlRecordEntityNameRequested(RouteData routeData, UrlRecordService.UrlRecordForCaching urlRecord)
        {
            RouteData = routeData;
            UrlRecord = urlRecord;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets information about the current routing path
        /// </summary>
        public RouteData RouteData { get; private set; }

        /// <summary>
        /// Gets or sets URL record
        /// </summary>
        public UrlRecordService.UrlRecordForCaching UrlRecord { get; private set; }

        #endregion

    }
}