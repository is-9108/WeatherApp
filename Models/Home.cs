using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Models
{
    public class Home
    {
        [Display(Name = "日時")]
        public string Date { get; set; }
        [Display(Name = "湿度")]
        public string Humidity { get; set; }
        [Display(Name = "気温")]
        public string Temp { get; set; }
        [Display(Name = "天気")]
        public string Description { get; set; }
        public string WeatherIcon { get; set; }
    }
}
