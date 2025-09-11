using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace WeatherApp.Controllers.API
{

    public class Pref
    {
        public string code { get; set; }
        public string name { get; set; }
        public string kana { get; set; }
    }
    public class City
    {
        public string code { get; set; }
        public string name { get; set; }
        public string kana { get; set; }

        public Pref pref;
    }
    
    [ApiController]
    [Route("api/[controller]")]
    public class GetCityController
    {
        private string apiUrl = "https://postal-codes-jp.azurewebsites.net/api/Cities/Pref/";
        [HttpPost("getCities")]
        public async Task<IActionResult> GetCities([FromForm] string id)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client
                .GetAsync(apiUrl + id);
            string result = await response.Content.ReadAsStringAsync();

            List<City>? citiesJson = JsonSerializer.Deserialize<List<City>>(result);
            var cities = citiesJson.Select(c => new { c.name }).ToList();
            
            if (response.IsSuccessStatusCode)
            {
                return new JsonResult(new { cities = cities });
            }

            return new JsonResult(new { message = "Error", id = id }); 
        }
    }
}
