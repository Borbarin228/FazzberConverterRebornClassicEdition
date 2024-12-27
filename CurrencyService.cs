using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Converter.Models;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Converter.Services
{
    public class CurrencyService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<List<CurrencyRate>> GetAvailableCurrenciesAsync()
        {
            try
            {
                
                var response = await _httpClient.GetStringAsync("https://www.cbr-xml-daily.ru/daily_json.js");

                
                var currencyData = JsonConvert.DeserializeObject<CurrencyData>(response);

                
                var rates = new List<CurrencyRate>(currencyData.Valute.Values);

                
                rates.Add(new CurrencyRate
                {
                    CharCode = "RUB",
                    Name = "Российский рубль",
                    Nominal = 1,
                    Value = 1
                });

                
                if (rates.Count == 0)
                {
                    throw new ApplicationException("Не удалось загрузить валюты из API.");
                }

                return rates;
            }
            catch (HttpRequestException e)
            {
                throw new ApplicationException("Ошибка при получении данных с API", e);
            }
            catch (Newtonsoft.Json.JsonException e)
            {
                throw new ApplicationException("Ошибка при парсинге данных JSON", e);
            }
        }
    }
}