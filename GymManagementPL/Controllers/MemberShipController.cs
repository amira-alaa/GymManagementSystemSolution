using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberShipViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MemberShipController : Controller
    {

        private readonly IMemberShipService _memberShipService;

        public MemberShipController(IMemberShipService memberShipService)
        {
            _memberShipService = memberShipService;
        }
        public ActionResult Index()
        {
            var memberShips = _memberShipService.GetAllMemberShips();
            return View(memberShips);
        }

        public ActionResult Create()
        {
            var members = _memberShipService.GetMembers();
            ViewBag.Members = new SelectList(members, "Id", "Name");
            var plans = _memberShipService.GetPlans();
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult CreateMemberShip(CreateMemberShipViewModel CreateMemberShip)
        {
            if (!ModelState.IsValid) RedirectToAction(nameof(Create), CreateMemberShip);

            bool IsCreated = _memberShipService.CreateMemberShip(CreateMemberShip);
            if (!IsCreated)
                TempData["Error"] = "Failed to create membership. Please try again.";
            return RedirectToAction(nameof(Index));


        }
        [HttpPost]
        public ActionResult Delete(int MemberId , int PlanId)
        {
            if(MemberId <= 0 || PlanId <= 0)
            {
                TempData["Error"] = "Invalid Membership Id.";
                return RedirectToAction(nameof(Index));
            }

            var membership = _memberShipService.GetMemberShipByIDs(MemberId, PlanId);
            if(membership == null)
            {
                TempData["Error"] = "Membership not found.";
                return RedirectToAction(nameof(Index));
            }
            bool IsDeleted = _memberShipService.DeleteMemberShip(membership);
            if (!IsDeleted)
                TempData["Error"] = "Failed to delete membership. Please try again.";
            else
                TempData["Success"] = "Membership deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
