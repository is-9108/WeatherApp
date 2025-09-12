namespace WeatherApp.Models
{
    public class WeatherForecastResponse
    {
        public List<Forecast> List { get; set; }
        public CityInfo City { get; set; }
    }

    public class Forecast
    {
        public long Dt { get; set; }
        public MainInfo Main { get; set; }
        public List<WeatherInfo> Weather { get; set; }
        public int Visibility { get; set; }
        public double Pop { get; set; }
        public string DtTxt { get; set; }
    }

    public class MainInfo
    {
        public double Temp { get; set; }
        public double Temp_min { get; set; }
        public double Temp_max { get; set; }
        public int Humidity { get; set; }
    }

    public class WeatherInfo
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class CityInfo
    {
        public string Name { get; set; }
    }
}
