using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetCityController : Controller
    {
        private string? UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);
        private readonly MydatabaseContext _context;
        public GetCityController(MydatabaseContext context)
        {
            _context = context;
        }

        [HttpPost("getCities")]
        public IActionResult GetCities([FromForm] string id)
        {
            string url = "https://postal-codes-jp.azurewebsites.net/api/Cities/Pref/" + id;

            string result = getApiInfo(url).Result;
            if (result == "Error")
                return new JsonResult(new { message = "Error" });
            List<string> cities = new List<string>();
            JsonDocument json = JsonDocument.Parse(result);
            JsonElement root = json.RootElement;
            foreach (var item in root.EnumerateArray())
            {
                cities.Add(item.GetProperty("name").GetString());
            }

            return new JsonResult(new { cities = cities });
        }


        [HttpPost("getLocate")]
        public async Task<IActionResult> GetLocate([FromForm] string todoufuken, [FromForm] string city)
        {
            string getTodoufukenUrl = "https://postal-codes-jp.azurewebsites.net/api/Prefs/" + todoufuken;
            var getCityUrl = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(city)}&format=json&limit=1&addressdetails=1";

            //緯度経度を取得
            string cityResult = getApiInfo(getCityUrl).Result;
            if (cityResult == "Error")
                return new JsonResult(new { message = "Error" });
            JsonDocument cityJson = JsonDocument.Parse(cityResult);
            JsonElement cityRoot = cityJson.RootElement;
            var cityIdo = cityRoot[0].GetProperty("lat").GetString();
            var cityKeido = cityRoot[0].GetProperty("lon").GetString();
           
            //都道府県名を取得
            string todoufukenResult = getApiInfo(getTodoufukenUrl).Result;
            if (todoufukenResult == "Error")
                return new JsonResult(new { message = "Error" });
            using JsonDocument todoufukenJson = JsonDocument.Parse(todoufukenResult);
            JsonElement todoufukenRoot = todoufukenJson.RootElement;
            var todoufukenName = todoufukenRoot.GetProperty("name").GetString();
            
            var items = _context.Locates.Where(I => I.UserId == UserId).ToList();
            

            if (items.Count <= 0)
            {
                Locate locate = new Locate()
                {
                    UserId = UserId,
                    Ido = double.Parse(cityIdo),
                    Keido = double.Parse(cityKeido)
                };

                _context.Locates.Add(locate);

            }
            else
            {
                items[0].Ido = double.Parse(cityIdo);
                items[0].Keido = double.Parse(cityKeido);
            }

            await _context.SaveChangesAsync();
            return new JsonResult(new { message="Success" });
        }

        public async Task<string> getApiInfo(string url)
        {
            string result = null;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "WeatherApp");
                HttpResponseMessage response = await client.GetAsync(url);
                result = await response.Content.ReadAsStringAsync();
            }

            if (result != null)
                return result;
            return "Error";

        }
    }
    
}
