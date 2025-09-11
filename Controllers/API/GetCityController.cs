using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace WeatherApp.Controllers.API
{
    public class City
    {
        public string code { get; set; }
        public string name { get; set; }
        public string kana { get; set; }
    }
    public class Locate
    {
        public string lat { get; set; }
        public string lon { get; set; }
    }
    
    [ApiController]
    [Route("api/[controller]")]
    public class GetCityController : Controller
    {
        private string? UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);
        [HttpPost("getCities")]
        public async Task<IActionResult> GetCities([FromForm] string id)
        {
            var url = "https://postal-codes-jp.azurewebsites.net/api/Cities/Pref/";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client
                .GetAsync(url + id);
            string result = await response.Content.ReadAsStringAsync();

            List<City>? citiesJson = JsonSerializer.Deserialize<List<City>>(result);
            var cities = citiesJson.Select(c => new { c.name }).ToList();
            
            if (response.IsSuccessStatusCode)
            {
                return new JsonResult(new { cities = cities });
            }

            return new JsonResult(new { message = "Error", id = id }); 
        }


        [HttpPost("getLocate")]
        public async Task<IActionResult> GetLocate([FromForm]string city)
        {
            var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(city)}&format=json&limit=1&addressdetails=1";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent","WeatherApp");
            HttpResponseMessage response = await client
                .GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
            List<Locate>? citiesJson = JsonSerializer.Deserialize<List<Locate>>(result);
            var ido = citiesJson[0].lat;
            var keido = citiesJson[0].lon;
            if(response.IsSuccessStatusCode)
            {
                return new JsonResult(new { userid = UserId, ido = ido, keido = keido });
            }
            return new JsonResult(new { message = "Error" });
        }
    }
    
}
