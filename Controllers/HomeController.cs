using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
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

            ViewData["Ido"] = userArea[0].Ido;
            ViewData["Keido"] = userArea[0].Keido;

            var url = $"https://api.openweathermap.org/data/2.5/forecast?lat={userArea[0].Ido}&lon={userArea[0].Keido}&appid={_apikey.WeatherKey}&units=metric&lang=ja";
            using (GetCityController GetCityController = new GetCityController(_context))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var result = GetCityController.getApiInfo(url).Result;
                var weatherInfos = JsonSerializer.Deserialize<WeatherForecastResponse>(result,options);
            }
                
            
            return View();
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
    }
}
