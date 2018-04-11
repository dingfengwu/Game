using Game.Base.Caching;
using Game.Base.Domain.Customers;
using Game.Services.Events;

namespace Game.Services.Customers.Cache
{
    /// <summary>
    /// Customer cache event consumer (used for caching of current customer password)
    /// </summary>
    public partial class CustomerCacheEventConsumer : IConsumer<CustomerPasswordChangedEvent>
    {
        #region Constants

        /// <summary>
        /// Key for current customer password lifetime
        /// </summary>
        /// <remarks>
        /// {0} : customer identifier
        /// </remarks>
        public const string CUSTOMER_PASSWORD_LIFETIME = "Game.customers.passwordlifetime-{0}";

        #endregion

        #region Fields

        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        public CustomerCacheEventConsumer(IStaticCacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        //password changed
        public void HandleEvent(CustomerPasswordChangedEvent eventMessage)
        {
            _cacheManager.Remove(string.Format(CUSTOMER_PASSWORD_LIFETIME, eventMessage.Password.CustomerId));
        }

        #endregion
    }
}
