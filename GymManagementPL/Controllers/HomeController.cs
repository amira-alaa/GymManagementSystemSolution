using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAnalyticesService _analyticesService;

        public HomeController(IAnalyticesService analyticesService)
        {
            _analyticesService = analyticesService;
        }

        public async Task<ActionResult> Index()
        {
            var data = await _analyticesService.GetAnalyticesDataAsync();
            return View(data);
        }
    }
}
