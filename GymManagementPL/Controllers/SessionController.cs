using System.Threading.Tasks;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<ActionResult> Index()
        {
            var sessions = await _sessionService.GetAllSessionsAsync();
            return View(sessions);
        }
        public async Task<ActionResult> GetSessionDetails(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Cannot Found Session ";
                return RedirectToAction(nameof(Index));
            }
            var session = await _sessionService.GetSessionByIdAsync(id);
            if (session is null)
            {
                TempData["Error"] = "Cannot Found Session ";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        public async Task<ActionResult> Create()
        {
            var trainers = await _sessionService.GetAllTrainerForDropDownAsync();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
            var categories = await _sessionService.GetAllCategoryForDropDownAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();

        }
        [HttpPost]
        public async Task<ActionResult> CreateSession(CreateSessionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Data ";
                return RedirectToAction(nameof(Index) , viewModel);
            }
            bool IsSessionCreated = await _sessionService.CreateSessionAsync(viewModel);
            if (!IsSessionCreated) TempData["Error"] = "Failed To Create Session ";
            else TempData["Success"] = "Session Created Successfully ";
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> UpdateSession(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Cannot Found Session ";
                return RedirectToAction(nameof(Index));
            }
            var session = await _sessionService.GetSessionForUpdateAsync(id);
            if (session is null)
            {
                TempData["Error"] = "Cannot Found Session ";
                return RedirectToAction(nameof(Index));
            }
            var trainers = await _sessionService.GetAllTrainerForDropDownAsync();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
            return View(session);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateSession(int id , UpdateSessionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Data ";
                return RedirectToAction(nameof(Index), viewModel);
            }
            bool IsSessionUpdated = await _sessionService.UpdateSessionAsync(viewModel, id);
            if (!IsSessionUpdated) TempData["Error"] = "Failed To Update Session ";
            else TempData["Success"] = "Session Updated Successfully ";
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Not Found Session";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionByIdAsync(id);
            if (session is null)
            {
                TempData["Error"] = "Not Found Session";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            bool IsSessionDeleted = await _sessionService.RemoveSessionAsync(id);
            if (!IsSessionDeleted)
            {
                TempData["Error"] = "Failed To Delete Session";
            }
            else
            {
                TempData["Success"] = "Session Deleted SuccessFully";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
