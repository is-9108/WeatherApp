using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Models;

public partial class Locate
{
    [Key]
    public string UserId { get; set; }

    [Display(Name = "緯度")]
    public double? Ido { get; set; }

    [Display(Name = "経度")]
    public double? Keido { get; set; }
}
