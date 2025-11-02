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

        public ActionResult Index()
        {
            var memberSessions = _memberSessionService.GetNotCompletedMemberSessions();
            return View(memberSessions);

        }

        public ActionResult GetMembersForOnGoingSessions(int sessionId)
        {
          
            var members = _memberSessionService.GetMembersForOnGoingsSessions(sessionId);
            return View(members);
        }

        [HttpPost]
        public ActionResult IsAttended(int sessionId , int memberId)
        {
            if(sessionId <=0 || memberId <=0)
                TempData["Error"] = "Invalid Data.";
            bool isAttended = _memberSessionService.IsAttended(sessionId, memberId);
            if(isAttended)
                TempData["Success"] = "Attendance status updated successfully.";
            else
                TempData["Error"] = "Failed to update attendance status.";

            ViewData["sessionId"] = sessionId;
            return RedirectToAction(nameof(GetMembersForOnGoingSessions), new { sessionId });
        }

        public ActionResult GetMembersForUpcomingSession(int sessionId)
        {
            var members = _memberSessionService.GetMembersForUpComingSessions(sessionId);
            TempData["SessionId"] = sessionId;
            TempData.Keep("SessionId");
            return View(members);
        }
        [HttpPost]
        public ActionResult Delete(int sessionId, int memberId)
        {
            if (sessionId <= 0 || memberId <= 0)
                TempData["Error"] = "Invalid Data.";
            var isDeleted = _memberSessionService.DeleteMemberSession(sessionId, memberId);
            if (isDeleted)
                TempData["Success"] = "Member removed from session successfully.";
            else
                TempData["Error"] = "Failed to remove member from session.";
            ViewData["sessionId"] = sessionId;
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { sessionId });
        }

        public ActionResult Create(int sessionId)
        {
            var members = _memberSessionService.GetMembers();
            ViewBag.Members = new SelectList(members, "MemberId", "MemberName");
           
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateMemberSessionViewModel createdMS )
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Data.";
                return View(createdMS);
            }
            var isCreated = _memberSessionService.CreateMemberSession(createdMS);

            if (isCreated)
                TempData["Success"] = "Member session created successfully.";
            else
                TempData["Error"] = "Failed to create member session.";
            //ViewData["sessionId"] = sessionId;
            return RedirectToAction(nameof(GetMembersForUpcomingSession));
        }
    }
}
