using irrigation_system.Models;
using Microsoft.AspNetCore.Mvc;

namespace irrigation_system.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Temperature = TemperatureStore.LastTemperature;
            return View();
        }
    }
}