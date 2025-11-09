using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberSessionViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MemberSessionController : Controller
    {
 
        private readonly IMemberSessionService _memberSessionService;

        public MemberSessionController(IMemberSessionService memberSessionService)
        {
            _memberSessionService = memberSessionService;
        }

        public async Task<ActionResult> Index()
        {
            var memberSessions = await _memberSessionService.GetNotCompletedMemberSessionsAsync();
            return View(memberSessions);

        }

        public async Task<ActionResult> GetMembersForOnGoingSessions(int sessionId)
        {
          
            var members = await _memberSessionService.GetMembersForOnGoingsSessionsAsync(sessionId);
            return View(members);
        }

        [HttpPost]
        public async Task<ActionResult> IsAttended(int sessionId , int memberId)
        {
            if(sessionId <=0 || memberId <=0)
                TempData["Error"] = "Invalid Data.";
            bool isAttended = await _memberSessionService.IsAttendedAsync(sessionId, memberId);
            if(isAttended)
                TempData["Success"] = "Attendance status updated successfully.";
            else
                TempData["Error"] = "Failed to update attendance status.";

            ViewData["sessionId"] = sessionId;
            return RedirectToAction(nameof(GetMembersForOnGoingSessions), new { sessionId });
        }

        public async Task<ActionResult> GetMembersForUpcomingSession(int sessionId)
        {
            var members = await _memberSessionService.GetMembersForUpComingSessionsAsync(sessionId);
            TempData["SessionId"] = sessionId;
            TempData.Keep("SessionId");
            return View(members);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int sessionId, int memberId)
        {
            if (sessionId <= 0 || memberId <= 0)
                TempData["Error"] = "Invalid Data.";
            var isDeleted = await _memberSessionService.DeleteMemberSessionAsync(sessionId, memberId);
            if (isDeleted)
                TempData["Success"] = "Member removed from session successfully.";
            else
                TempData["Error"] = "Failed to remove member from session.";
            ViewData["sessionId"] = sessionId;
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { sessionId });
        }

        public async Task<ActionResult> Create(int sessionId)
        {
            var members = await _memberSessionService.GetMembersAsync();
            ViewBag.Members = new SelectList(members, "MemberId", "MemberName");
           
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateMemberSessionViewModel createdMS )
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Data.";
                return View(createdMS);
            }
            var isCreated = await _memberSessionService.CreateMemberSessionAsync(createdMS);

            if (isCreated)
                TempData["Success"] = "Member session created successfully.";
            else
                TempData["Error"] = "Failed to create member session.";
            //ViewData["sessionId"] = sessionId;
            return RedirectToAction(nameof(GetMembersForUpcomingSession));
        }
    }
}
