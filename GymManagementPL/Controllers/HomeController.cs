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

        public ActionResult Index()
        {
            var data = _analyticesService.GetAnalyticesData();
            return View(data);
        }
    }
}
