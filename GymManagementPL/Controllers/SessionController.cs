using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public ActionResult Index()
        {
            var sessions = _sessionService.Index();
            return View(sessions);
        }
        public ActionResult GetSessionDetails(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Cannot Found Session ";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["Error"] = "Cannot Found Session ";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        public ActionResult Create()
        {
            var trainers = _sessionService.GetAllTrainerForDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
            var categories = _sessionService.GetAllCategoryForDropDown();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();

        }
        [HttpPost]
        public ActionResult CreateSession(CreateSessionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Data ";
                return RedirectToAction(nameof(Index) , viewModel);
            }
            bool IsSessionCreated = _sessionService.CreateSession(viewModel);
            if (!IsSessionCreated) TempData["Error"] = "Failed To Create Session ";
            else TempData["Success"] = "Session Created Successfully ";
            return RedirectToAction(nameof(Index));
        }

        public ActionResult UpdateSession(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Cannot Found Session ";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionForUpdate(id);
            if (session is null)
            {
                TempData["Error"] = "Cannot Found Session ";
                return RedirectToAction(nameof(Index));
            }
            var trainers = _sessionService.GetAllTrainerForDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
            return View(session);
        }
        [HttpPost]
        public ActionResult UpdateSession(int id , UpdateSessionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Data ";
                return RedirectToAction(nameof(Index), viewModel);
            }
            bool IsSessionUpdated = _sessionService.UpdateSession(viewModel, id);
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
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["Error"] = "Not Found Session";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            bool IsSessionDeleted = _sessionService.RemoveSession(id);
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
