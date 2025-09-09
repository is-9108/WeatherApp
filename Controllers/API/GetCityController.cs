using Microsoft.AspNetCore.Mvc;

namespace WeatherApp.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetCityController
    {
        [HttpPost("getCities")]
        public IActionResult GetCities([FromForm] int id)
        {
            // 都道府県IDに基づく市区町村リスト取得処理
            return new JsonResult(new { message = "Success", id = id });// 仮のレスポンス 
        }
    }
}
