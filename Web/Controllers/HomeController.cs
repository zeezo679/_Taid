using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Models.Interfaces;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICrsResultRepository  _crsResultRepository;

        public HomeController(ILogger<HomeController> logger,  ICrsResultRepository crsResultRepository)
        {
            _logger = logger;
            _crsResultRepository = crsResultRepository;
        }

        public async Task<IActionResult> Index()
        {
            var filteredCoursesByCurrentUser = await _crsResultRepository
                .FilterCoursesByCurrentUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            return View(filteredCoursesByCurrentUser);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
