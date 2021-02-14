using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [ActivatorUtilitiesConstructor]
        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        private async Task<WeatherForecast> GetWeatherForecasts()
        {
           var client = _clientFactory.CreateClient("API Client");
            var result = await client.GetAsync("/api/location/906057/");
           
            if (result.IsSuccessStatusCode)
            {
                // Read all of the response and deserialise it into an instace of
                // WeatherForecast class
                var content = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<WeatherForecast>(content);
            }
            return null;
        }

        public async Task<IActionResult> Index()
        {
            var model = await GetWeatherForecasts();
            // Pass the data into the View
            return View(model);
        }
    }
}

