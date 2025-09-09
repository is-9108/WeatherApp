using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MydatabaseContext _context;
        private string? UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public HomeController(ILogger<HomeController> logger, MydatabaseContext context)
        {
            _logger = logger;
            _context = context;

        }

        [Authorize]
        public IActionResult Index()
        {

            var userArea = _context.Locates.Where(l => l.UserId == UserId).ToList();
            if (userArea.Count() == 0)
            {
                return RedirectToAction("RegisterArea");
            }
            var ido = userArea.Select(u => u.Ido);
            var keido = userArea.Select(u => u.Keido);

            if (ido == null || keido == null)
                return RedirectToAction("RegisterArea");



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
