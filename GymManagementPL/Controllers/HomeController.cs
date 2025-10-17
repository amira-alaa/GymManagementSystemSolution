using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
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
