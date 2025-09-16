using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using WeatherApp.Controllers.API;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    


    public class HomeController : Controller
    {
        private readonly MydatabaseContext _context;
        private readonly APIKey _apikey;
        private string? UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public HomeController(MydatabaseContext context, IOptions<APIKey> options)
        {
            _context = context;
            _apikey = options.Value;

        }

        [Authorize]
        public IActionResult Index()
        {
           
            var userArea = _context.Locates.Where(l => l.UserId == UserId).ToList();
            if (userArea.Count() == 0)
                return RedirectToAction("RegisterArea");

            bool isHanging = true;
            List<Forecast> forecastList = new List<Forecast>();
            var url = $"https://api.openweathermap.org/data/2.5/forecast?lat={userArea[0].Ido}&lon={userArea[0].Keido}&appid={_apikey.WeatherKey}&units=metric&lang=ja";
            using (GetCityController GetCityController = new GetCityController(_context))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var result = GetCityController.getApiInfo(url).Result;
                var weatherInfos = JsonSerializer.Deserialize<WeatherForecastResponse>(result,options);
                foreach(var forecast in weatherInfos.List)
                {
                    long date = forecast.Dt;
                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(date);
                    DateTimeOffset japanDateTime = dateTimeOffset.ToOffset(TimeSpan.FromHours(9));
                    forecast.DtTxt = japanDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    forecastList.Add(forecast);
                }
            }
            
            List<Home> homes = new List<Home>();
            for(int i = 0; i < forecastList.Count; i++)
            {
                homes.Add(new Home
                {
                    Date = forecastList[i].DtTxt,
                    Humidity = forecastList[i].Main.Humidity.ToString(),
                    Temp = forecastList[i].Main.Temp.ToString(),
                    Description = forecastList[i].Weather[0].Description,
                    WeatherIcon = GetWeatherIconUrl(forecastList[i].Weather[0].Icon)
                });
            }

            for(int i = 0; i < 3; i++)
            {
                if (forecastList[i].Weather[0].Id < 800)
                {
                    isHanging = false;
                    break;
                }
            }
            ViewData["isHanging"] = isHanging;
            return View(homes);
        }

        public async Task<IActionResult> RegisterArea()
        {
            var cities = await _context.Cities.ToListAsync();
            return View(cities);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        public string GetWeatherIconUrl(string IconId)
        {
            return $"https://openweathermap.org/img/wn/{IconId.Remove(IconId.Length - 1)}d@2x.png";
        }
    }
}
