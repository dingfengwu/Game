using System.Collections.Generic;
using Game.Base.Domain.Directory;

namespace Game.Services.Directory
{
    /// <summary>
    /// Exchange rate provider interface
    /// </summary>
    public partial interface IExchangeRateProvider
    {
        /// <summary>
        /// Gets currency live rates
        /// </summary>
        /// <param name="exchangeRateCurrencyCode">Exchange rate currency code</param>
        /// <returns>Exchange rates</returns>
        IList<ExchangeRate> GetCurrencyLiveRates(string exchangeRateCurrencyCode);
    }
}