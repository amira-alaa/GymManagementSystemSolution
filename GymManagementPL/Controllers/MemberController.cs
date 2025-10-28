using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public ActionResult Index()
        {
            // get all data
            var members = _memberService.Index();
            return View(members);
        }
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Cannot Found Member ";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMember(id);
            if (member is null)
            {
                TempData["Error"] = "Cannot Found Member ";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Cannot Found HealthRecord ";
                return RedirectToAction(nameof(Index)); 
            }
            var healthRecord = _memberService.GetHealthRecord(id);
            if (healthRecord is null)
            {
                TempData["Error"] = "Cannot Found HealthRecord ";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel viewModel )
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data. Please check the input fields.";
                return RedirectToAction(nameof(Create) , viewModel);
            }
            bool IsMemberCreated = _memberService.Create(viewModel);
            if (!IsMemberCreated)
            {
                TempData["Error"] = "Failed to create member. Please try again.";
            }
            else
            {
                TempData["Success"] = "Member created successfully.";
            }
             return RedirectToAction(nameof(Index));

        }

        public ActionResult UpdateMember(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Cannot Found Member ";
                return RedirectToAction(nameof(Index));
            }
            var memberToUpdate = _memberService.GetMemberToUpdate(id);
            if (memberToUpdate is null)
            {
                TempData["Error"] = "Cannot Found Member ";
                return RedirectToAction(nameof(Index));
            }
            return View(memberToUpdate);
        }
        [HttpPost]
        public ActionResult UpdateMember(int id , MemberToUpdateViewModel viewModel)
        {
            if(!ModelState.IsValid) return View(viewModel);
            bool IsMemberUpdated = _memberService.UpdateMemberDetails(id , viewModel);
            if(!IsMemberUpdated)
            {
                TempData["Error"] = "Failed to update member details. Please try again.";
            }
            else
            {
                TempData["Success"] = "Member details updated successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Cannot Found Member ";
                return RedirectToAction(nameof(Index));
            }
            var memberToUpdate = _memberService.GetMemberToUpdate(id);
            if (memberToUpdate is null)
            {
                TempData["Error"] = "Cannot Found Member ";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            bool IsMemberDeleted = _memberService.RemoveMember(id);
            if (!IsMemberDeleted)
            {
                TempData["Error"] = "Failed to delete member. Please try again.";
            }
            else
            {
                TempData["Success"] = "Member deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
