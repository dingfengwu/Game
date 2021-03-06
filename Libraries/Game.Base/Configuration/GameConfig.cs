using System;
using System.Configuration;
using System.Xml;

namespace Game.Base.Configuration
{
    /// <summary>
    /// Represents a GameConfig
    /// </summary>
    public partial class GameConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether to display the full error in production environment.
        /// It's ignored (always enabled) in development environment
        /// </summary>
        public bool DisplayFullErrorStack { get; set; }

        /// <summary>
        /// Gets or sets a value of "Cache-Control" header value for static content
        /// </summary>
        public string StaticFilesCacheControl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we compress response
        /// </summary>
        public bool UseResponseCompression { get; set; }

        /// <summary>
        /// Gets or sets connection string for Azure BLOB storage
        /// </summary>
        public string AzureBlobStorageConnectionString { get; set; }
        /// <summary>
        /// Gets or sets container name for Azure BLOB storage
        /// </summary>
        public string AzureBlobStorageContainerName { get; set; }
        /// <summary>
        /// Gets or sets end point for Azure BLOB storage
        /// </summary>
        public string AzureBlobStorageEndPoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should use Redis server for caching (instead of default in-memory caching)
        /// </summary>
        public bool RedisCachingEnabled { get; set; }
        /// <summary>
        /// Gets or sets Redis connection string. Used when Redis caching is enabled
        /// </summary>
        public string RedisCachingConnectionString { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the data protection system should be configured to persist keys in the Redis database
        /// </summary>
        public bool PersistDataProtectionKeysToRedis { get; set; }

        /// <summary>
        /// Gets or sets path to database with user agent strings
        /// </summary>
        public string UserAgentStringsPath { get; set; }
        /// <summary>
        /// Gets or sets path to database with crawler only user agent strings
        /// </summary>
        public string CrawlerOnlyUserAgentStringsPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should support previous gameCommerce versions (it can slightly improve performance)
        /// </summary>
        public bool SupportPreviousGamecommerceVersions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a store owner can install sample data during installation
        /// </summary>
        public bool DisableSampleDataDuringInstallation { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to use fast installation. 
        /// By default this setting should always be set to "False" (only for advanced users)
        /// </summary>
        public bool UseFastInstallationService { get; set; }
        /// <summary>
        /// Gets or sets a list of plugins ignored during gameCommerce installation
        /// </summary>
        public string PluginsIgnoredDuringInstallation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should ignore startup tasks
        /// </summary>
        public bool IgnoreStartupTasks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to clear /Plugins/bin directory on application startup
        /// </summary>
        public bool ClearPluginShadowDirectoryOnStartup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to load an assembly into the load-from context, bypassing some security checks.
        /// </summary>
        public bool UseUnsafeLoadAssembly { get; set; }

        /// <summary>
        /// 是否显示调试工具
        /// </summary>
        public bool DisplayMiniProfiler { get; set; }

        /// <summary>
        /// 是否只有管理成员才显示调试工具
        /// </summary>
        public bool DisplayMiniProfilerForAdminOnly { get; set; }
    }
}
