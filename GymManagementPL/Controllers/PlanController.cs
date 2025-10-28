using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        public ActionResult Index()
        {
            var plans = _planService.Index();
            return View(plans);
        }

        public ActionResult GetPlanDetails(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Not Found Plan";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanDetails(id);
            if (plan is null)
            {
                TempData["Error"] = "Not Found Plan";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        public ActionResult UpdatePlan(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Not Found Plan";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["Error"] = "Not Found Plan";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpPost]
        public ActionResult UpdatePlan(int id , UpdatePlanViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Data";
                return View(viewModel);
            }
            bool IsPlanUpdated = _planService.UpdatePlan(id , viewModel);
            if (IsPlanUpdated)
            {
                TempData["Success"] = "Plan Updated Successfully";
            }
            else
            {
                TempData["Error"] = "Failed to Update Plan";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Activate(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Not Found Plan";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.TogglePlanStatus(id);
            if (!plan)
            {
                TempData["Error"] = "Cannot Activate Plan";
            }
            else
            {
                TempData["Success"] = "Plan Activated Successfully";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
