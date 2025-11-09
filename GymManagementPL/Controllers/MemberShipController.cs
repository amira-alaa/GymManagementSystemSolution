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

        public async Task<ActionResult> Create()
        {
            var members = await _memberShipService.GetMembersAsync();
            ViewBag.Members = new SelectList(members, "Id", "Name");
            var plans = await _memberShipService.GetPlansAsync();
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateMemberShip(CreateMemberShipViewModel CreateMemberShip)
        {
            if (!ModelState.IsValid) RedirectToAction(nameof(Create), CreateMemberShip);

            bool IsCreated = await _memberShipService.CreateMemberShipAsync(CreateMemberShip);
            if (!IsCreated)
                TempData["Error"] = "Failed to create membership. Please try again.";
            return RedirectToAction(nameof(Index));


        }
        [HttpPost]
        public async Task<ActionResult> Delete(int MemberId , int PlanId)
        {
            if(MemberId <= 0 || PlanId <= 0)
            {
                TempData["Error"] = "Invalid Membership Id.";
                return RedirectToAction(nameof(Index));
            }

            var membership = await _memberShipService.GetMemberShipByIDsAsync(MemberId, PlanId);
            if(membership == null)
            {
                TempData["Error"] = "Membership not found.";
                return RedirectToAction(nameof(Index));
            }
            bool IsDeleted = await _memberShipService.DeleteMemberShipAsync(membership);
            if (!IsDeleted)
                TempData["Error"] = "Failed to delete membership. Please try again.";
            else
                TempData["Success"] = "Membership deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
