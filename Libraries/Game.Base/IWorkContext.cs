using Game.Base.Domain.Customers;
using Game.Base.Domain.Localization;

namespace Game.Base
{
    /// <summary>
    /// Represents work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        Customer CurrentCustomer { get; set; }

        /// <summary>
        /// Gets or sets current user working language
        /// </summary>
        Language WorkingLanguage { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        Customer OriginalCustomerIfImpersonated { get; }
    }
}
