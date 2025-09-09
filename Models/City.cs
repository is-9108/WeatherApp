using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Models;

public partial class City
{
    [Display(Name = "都道府県コード")]
    public string Id { get; set; } = null!;

    [Display(Name = "都道府県")]
    public string CityName { get; set; } = null!;

}
