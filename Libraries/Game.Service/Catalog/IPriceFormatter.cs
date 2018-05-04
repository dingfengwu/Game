using Game.Base.Domain.Directory;
using Game.Base.Domain.Localization;
using Game.Base.Domain.Matches;

namespace Game.Services.Catalog
{
    /// <summary>
    /// Price formatter
    /// </summary>
    public partial interface IPriceFormatter
    {
        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <returns>Price</returns>
        string FormatPrice(decimal price);

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <returns>Price</returns>
        string FormatPrice(decimal price, bool showCurrency, Currency targetCurrency);

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="showTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        string FormatPrice(decimal price, bool showCurrency);

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="currencyCode">Currency code</param>
        /// <param name="showTax">A value indicating whether to show tax suffix</param>
        /// <param name="language">Language</param>
        /// <returns>Price</returns>
        string FormatPrice(decimal price, bool showCurrency, 
            string currencyCode, Language language);
        
        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <param name="showTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        string FormatPrice(decimal price, bool showCurrency, 
            Currency targetCurrency, Language language);
        
        /// <summary>
        /// Formats a tax rate
        /// </summary>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Formatted tax rate</returns>
        string FormatTaxRate(decimal taxRate);
    }
}
