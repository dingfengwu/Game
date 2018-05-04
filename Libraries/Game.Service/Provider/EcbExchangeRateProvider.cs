﻿using Game.Base;
using Game.Base.Domain.Directory;
using Game.Services.Directory;
using Game.Services.Localization;
using Game.Services.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;

namespace Game.Services.Provider
{
    public class EcbExchangeRateProvider : IExchangeRateProvider
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public EcbExchangeRateProvider(ILocalizationService localizationService,
            ILogger logger)
        {
            this._localizationService = localizationService;
            this._logger = logger;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets currency live rates
        /// </summary>
        /// <param name="exchangeRateCurrencyCode">Exchange rate currency code</param>
        /// <returns>Exchange rates</returns>
        public IList<ExchangeRate> GetCurrencyLiveRates(string exchangeRateCurrencyCode)
        {
            if (exchangeRateCurrencyCode == null)
                throw new ArgumentNullException(nameof(exchangeRateCurrencyCode));

            //add euro with rate 1
            var ratesToEuro = new List<ExchangeRate>
            {
                new ExchangeRate
                {
                    CurrencyCode = "EUR",
                    Rate = 1,
                    UpdatedOn = DateTime.UtcNow
                }
            };

            //get exchange rates to euro from European Central Bank
            var request = (HttpWebRequest)WebRequest.Create("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml");
            try
            {
                using (var response = request.GetResponse())
                {
                    //load XML document
                    var document = new XmlDocument();
                    document.Load(response.GetResponseStream());

                    //add namespaces
                    var namespaces = new XmlNamespaceManager(document.NameTable);
                    namespaces.AddNamespace("ns", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
                    namespaces.AddNamespace("gesmes", "http://www.gesmes.org/xml/2002-08-01");

                    //get daily rates
                    var dailyRates = document.SelectSingleNode("gesmes:Envelope/ns:Cube/ns:Cube", namespaces);
                    if (!DateTime.TryParseExact(dailyRates.Attributes["time"].Value, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime updateDate))
                        updateDate = DateTime.UtcNow;

                    foreach (XmlNode currency in dailyRates.ChildNodes)
                    {
                        //get rate
                        if (!decimal.TryParse(currency.Attributes["rate"].Value, out decimal currencyRate))
                            continue;

                        ratesToEuro.Add(new ExchangeRate()
                        {
                            CurrencyCode = currency.Attributes["currency"].Value,
                            Rate = currencyRate,
                            UpdatedOn = updateDate
                        });
                    }
                }
            }
            catch (WebException ex)
            {
                _logger.Error("ECB exchange rate provider", ex);
            }

            //return result for the euro
            if (exchangeRateCurrencyCode.Equals("eur", StringComparison.InvariantCultureIgnoreCase))
                return ratesToEuro;

            //use only currencies that are supported by ECB
            var exchangeRateCurrency = ratesToEuro.FirstOrDefault(rate => rate.CurrencyCode.Equals(exchangeRateCurrencyCode, StringComparison.InvariantCultureIgnoreCase));
            if (exchangeRateCurrency == null)
                throw new GameException(_localizationService.GetResource("Plugins.ExchangeRate.EcbExchange.Error"));

            //return result for the selected (not euro) currency
            return ratesToEuro.Select(rate => new ExchangeRate
            {
                CurrencyCode = rate.CurrencyCode,
                Rate = Math.Round(rate.Rate / exchangeRateCurrency.Rate, 4),
                UpdatedOn = rate.UpdatedOn
            }).ToList();
        }
        
        #endregion

    }
}
