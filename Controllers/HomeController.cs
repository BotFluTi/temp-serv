using irrigation_system.Data;
using Microsoft.AspNetCore.Mvc;

namespace irrigation_system.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var lastReading = _context.TemperatureReadings
                .OrderByDescending(x => x.ReadAt)
                .FirstOrDefault();

            if (lastReading == null)
            {
                ViewBag.Temperature = "No data yet";
                ViewBag.ReadAt = null;
            }
            else
            {
                ViewBag.Temperature = lastReading.Temperature.ToString("0.0");
                ViewBag.ReadAt = lastReading.ReadAt;
            }

            return View();
        }
    }
}