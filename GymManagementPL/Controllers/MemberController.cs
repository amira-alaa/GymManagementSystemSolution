using System.Threading.Tasks;
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
        public async Task<ActionResult> Index()
        {
            // get all data
            var members = await _memberService.GetAllMembersAsync();
            return View(members);
        }
        public async Task<ActionResult> MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Cannot Found Member ";
                return RedirectToAction(nameof(Index));
            }
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member is null)
            {
                TempData["Error"] = "Cannot Found Member ";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        public async Task<ActionResult> HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Cannot Found HealthRecord ";
                return RedirectToAction(nameof(Index)); 
            }
            var healthRecord = await _memberService.GetHealthRecordAsync(id);
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
        public async Task<ActionResult> CreateMember(CreateMemberViewModel viewModel )
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data. Please check the input fields.";
                return RedirectToAction(nameof(Create) , viewModel);
            }
            bool IsMemberCreated = await _memberService.CreateMemberAsync(viewModel);
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

        public async Task<ActionResult> UpdateMember(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Cannot Found Member ";
                return RedirectToAction(nameof(Index));
            }
            var memberToUpdate = await _memberService.GetMemberToUpdateAsync(id);
            if (memberToUpdate is null)
            {
                TempData["Error"] = "Cannot Found Member ";
                return RedirectToAction(nameof(Index));
            }
            return View(memberToUpdate);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateMember(int id , MemberToUpdateViewModel viewModel)
        {
            if(!ModelState.IsValid) return View(viewModel);
            bool IsMemberUpdated = await _memberService.UpdateMemberDetailsAsync(id , viewModel);
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

        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Cannot Found Member ";
                return RedirectToAction(nameof(Index));
            }
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member is null)
            {
                TempData["Error"] = "Cannot Found Member ";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            bool IsMemberDeleted = await _memberService.RemoveMemberAsync(id);
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
