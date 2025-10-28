using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        public ActionResult Index()
        {
            var trainers = _trainerService.Index();
            return View(trainers);
        }
        public ActionResult GetTrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Not Found Trainer";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainer(id);
            if (trainer is null)
            {
                TempData["Error"] = "Not Found Trainer";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateTrainer(CreateTrainerViewModel CreatedTrainer)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Data";
                return RedirectToAction(nameof(Create), CreatedTrainer);
            }
            bool IsTrainerCreated = _trainerService.Create(CreatedTrainer);
            if (IsTrainerCreated)
            {
                TempData["Success"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["Error"] = "Failed to Create Trainer";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult UpdateTrainer(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Not Found Trainer";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerToUpdate(id);
            if (trainer is null)
            {
                TempData["Error"] = "Not Found Trainer";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        [HttpPost]
        public ActionResult UpdateTrainer(int id, TrainerToUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Data";
                return View(viewModel);
            }
            bool IsTrainerUpdated = _trainerService.UpdateTrainerDetails(id, viewModel);
            if (!IsTrainerUpdated)
            {
                TempData["Error"] = "Failed to Update Trainer";
            }
            else
            {
                TempData["Success"] = "Trainer Updated Successfully";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Not Found Trainer";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainer(id);
            if (trainer is null)
            {
                TempData["Error"] = "Not Found Trainer";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            bool IsTrainerDeleted = _trainerService.RemoveTrainer(id);
            if (!IsTrainerDeleted)
            {
                TempData["Error"] = "Failed To Delete Trainer";
            }
            else
            {
                TempData["Success"] = "Trainer Deleted SuccessFully";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
