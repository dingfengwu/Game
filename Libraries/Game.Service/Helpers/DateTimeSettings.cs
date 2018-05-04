using Game.Base.Configuration;

namespace Game.Services.Helpers
{
    /// <summary>
    /// DateTime settings
    /// </summary>
    public class DateTimeSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a default store time zone identifier
        /// </summary>
        public string DefaultPlatformTimeZoneId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to select theirs time zone
        /// </summary>
        public bool AllowCustomersToSetTimeZone { get; set; }
    }
}