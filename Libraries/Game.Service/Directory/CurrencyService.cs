using Game.Base;
using Game.Base.Caching;
using Game.Base.Data;
using Game.Base.Domain.Customers;
using Game.Base.Domain.Directory;
using Game.Base.Infrastructure;
using Game.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Services.Directory
{
    /// <summary>
    /// Currency service
    /// </summary>
    public partial class CurrencyService : ICurrencyService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : currency ID
        /// </remarks>
        private const string CURRENCIES_BY_ID_KEY = "Game.currency.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string CURRENCIES_ALL_KEY = "Game.currency.all-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CURRENCIES_PATTERN_KEY = "Game.currency.";

        #endregion

        #region Fields

        private readonly IRepository<Currency> _currencyRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly CurrencySettings _currencySettings;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="currencyRepository">Currency repository</param>
        /// <param name="storeMappingService">Store mapping service</param>
        /// <param name="currencySettings">Currency settings</param>
        /// <param name="pluginFinder">Plugin finder</param>
        /// <param name="eventPublisher">Event published</param>
        public CurrencyService(IStaticCacheManager cacheManager,
            IRepository<Currency> currencyRepository,
            CurrencySettings currencySettings,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._currencyRepository = currencyRepository;
            this._currencySettings = currencySettings;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        #region Currency

        /// <summary>
        /// Gets currency live rates
        /// </summary>
        /// <param name="exchangeRateCurrencyCode">Exchange rate currency code</param>
        /// <param name="customer">Load records allowed only to a specified customer; pass null to ignore ACL permissions</param>
        /// <returns>Exchange rates</returns>
        public virtual IList<ExchangeRate> GetCurrencyLiveRates(string exchangeRateCurrencyCode, Customer customer = null)
        {
            var exchangeRateProvider = EngineContext.Current.Resolve<IExchangeRateProvider>();
            if (exchangeRateProvider == null)
                throw new Exception("Active exchange rate provider cannot be loaded");

            return exchangeRateProvider.GetCurrencyLiveRates(exchangeRateCurrencyCode);
        }

        /// <summary>
        /// Deletes currency
        /// </summary>
        /// <param name="currency">Currency</param>
        public virtual void DeleteCurrency(Currency currency)
        {
            if (currency == null)
                throw new ArgumentNullException(nameof(currency));

            if (currency is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _currencyRepository.Delete(currency);

            _cacheManager.RemoveByPattern(CURRENCIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(currency);
        }

        /// <summary>
        /// Gets a currency
        /// </summary>
        /// <param name="currencyId">Currency identifier</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Currency</returns>
        public virtual Currency GetCurrencyById(int currencyId, bool loadCacheableCopy = true)
        {
            if (currencyId == 0)
                return null;

            Func<Currency> loadCurrencyFunc = () =>
            {
                return _currencyRepository.GetById(currencyId);
            };

            if (loadCacheableCopy)
            {
                //cacheable copy
                var key = string.Format(CURRENCIES_BY_ID_KEY, currencyId);
                return _cacheManager.Get(key, () =>
                {
                    var currency = loadCurrencyFunc();
                    if (currency == null)
                        return null;
                    return new CurrencyForCaching(currency);
                });
            }
            return loadCurrencyFunc();
        }

        /// <summary>
        /// Gets a currency by code
        /// </summary>
        /// <param name="currencyCode">Currency code</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Currency</returns>
        public virtual Currency GetCurrencyByCode(string currencyCode, bool loadCacheableCopy = true)
        {
            if (string.IsNullOrEmpty(currencyCode))
                return null;
            return GetAllCurrencies(true, loadCacheableCopy: loadCacheableCopy)
                .FirstOrDefault(c => c.CurrencyCode.ToLower() == currencyCode.ToLower());
        }

        /// <summary>
        /// Gets all currencies
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Currencies</returns>
        public virtual IList<Currency> GetAllCurrencies(bool showHidden = false, bool loadCacheableCopy = true)
        {
            Func<IList<Currency>> loadCurrenciesFunc = () =>
            {
                var query = _currencyRepository.Table;
                if (!showHidden)
                    query = query.Where(c => c.Published);
                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id);
                return query.ToList();
            };

            IList<Currency> currencies;
            if (loadCacheableCopy)
            {
                //cacheable copy
                var key = string.Format(CURRENCIES_ALL_KEY, showHidden);
                currencies = _cacheManager.Get(key, () =>
                {
                    var result = new List<Currency>();
                    foreach (var currency in loadCurrenciesFunc())
                        result.Add(new CurrencyForCaching(currency));
                    return result;
                });
            }
            else
            {
                currencies = loadCurrenciesFunc();
            }
            return currencies;
        }

        /// <summary>
        /// Inserts a currency
        /// </summary>
        /// <param name="currency">Currency</param>
        public virtual void InsertCurrency(Currency currency)
        {
            if (currency == null)
                throw new ArgumentNullException(nameof(currency));

            if (currency is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _currencyRepository.Insert(currency);

            _cacheManager.RemoveByPattern(CURRENCIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(currency);
        }

        /// <summary>
        /// Updates the currency
        /// </summary>
        /// <param name="currency">Currency</param>
        public virtual void UpdateCurrency(Currency currency)
        {
            if (currency == null)
                throw new ArgumentNullException(nameof(currency));

            if (currency is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _currencyRepository.Update(currency);

            _cacheManager.RemoveByPattern(CURRENCIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(currency);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="exchangeRate">Currency exchange rate</param>
        /// <returns>Converted value</returns>
        public virtual decimal ConvertCurrency(decimal amount, decimal exchangeRate)
        {
            if (amount != decimal.Zero && exchangeRate != decimal.Zero)
                return amount * exchangeRate;
            return decimal.Zero;
        }

        /// <summary>
        /// Converts currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCurrencyCode">Source currency code</param>
        /// <param name="targetCurrencyCode">Target currency code</param>
        /// <returns>Converted value</returns>
        public virtual decimal ConvertCurrency(decimal amount, Currency sourceCurrencyCode, Currency targetCurrencyCode)
        {
            if (sourceCurrencyCode == null)
                throw new ArgumentNullException(nameof(sourceCurrencyCode));

            if (targetCurrencyCode == null)
                throw new ArgumentNullException(nameof(targetCurrencyCode));

            var result = amount;
            if (sourceCurrencyCode.Id == targetCurrencyCode.Id)
                return result;
            if (result != decimal.Zero && sourceCurrencyCode.Id != targetCurrencyCode.Id)
            {
                result = ConvertToPrimaryExchangeRateCurrency(result, sourceCurrencyCode);
                result = ConvertFromPrimaryExchangeRateCurrency(result, targetCurrencyCode);
            }
            return result;
        }

        /// <summary>
        /// Converts to primary exchange rate currency 
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCurrencyCode">Source currency code</param>
        /// <returns>Converted value</returns>
        public virtual decimal ConvertToPrimaryExchangeRateCurrency(decimal amount, Currency sourceCurrencyCode)
        {
            if (sourceCurrencyCode == null)
                throw new ArgumentNullException(nameof(sourceCurrencyCode));

            var primaryExchangeRateCurrency = GetCurrencyById(_currencySettings.PrimaryExchangeRateCurrencyId);
            if (primaryExchangeRateCurrency == null)
                throw new Exception("Primary exchange rate currency cannot be loaded");

            var result = amount; 
            if (result != decimal.Zero && sourceCurrencyCode.Id != primaryExchangeRateCurrency.Id)
            {
                var exchangeRate = sourceCurrencyCode.Rate;
                if (exchangeRate == decimal.Zero)
                    throw new GameException($"Exchange rate not found for currency [{sourceCurrencyCode.Name}]");
                result = result / exchangeRate;
            }
            return result;
        }

        /// <summary>
        /// Converts from primary exchange rate currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="targetCurrencyCode">Target currency code</param>
        /// <returns>Converted value</returns>
        public virtual decimal ConvertFromPrimaryExchangeRateCurrency(decimal amount, Currency targetCurrencyCode)
        {
            if (targetCurrencyCode == null)
                throw new ArgumentNullException(nameof(targetCurrencyCode));

            var primaryExchangeRateCurrency = GetCurrencyById(_currencySettings.PrimaryExchangeRateCurrencyId);
            if (primaryExchangeRateCurrency == null)
                throw new Exception("Primary exchange rate currency cannot be loaded");

            var result = amount;
            if (result != decimal.Zero && targetCurrencyCode.Id != primaryExchangeRateCurrency.Id)
            {
                var exchangeRate = targetCurrencyCode.Rate;
                if (exchangeRate == decimal.Zero)
                    throw new GameException($"Exchange rate not found for currency [{targetCurrencyCode.Name}]");
                result = result * exchangeRate;
            }
            return result;
        }

        /// <summary>
        /// Converts to primary store currency 
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCurrencyCode">Source currency code</param>
        /// <returns>Converted value</returns>
        public virtual decimal ConvertToPrimaryStoreCurrency(decimal amount, Currency sourceCurrencyCode)
        {
            if (sourceCurrencyCode == null)
                throw new ArgumentNullException(nameof(sourceCurrencyCode));

            var primaryStoreCurrency = GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId);
            var result = ConvertCurrency(amount, sourceCurrencyCode, primaryStoreCurrency);
            return result;
        }

        /// <summary>
        /// Converts from primary store currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="targetCurrencyCode">Target currency code</param>
        /// <returns>Converted value</returns>
        public virtual decimal ConvertFromPrimaryStoreCurrency(decimal amount, Currency targetCurrencyCode)
        {
            var primaryStoreCurrency = GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId);
            var result = ConvertCurrency(amount, primaryStoreCurrency, targetCurrencyCode);
            return result;
        }

        #endregion

        #endregion
    }
}