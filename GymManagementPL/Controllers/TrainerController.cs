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
        public async Task<ActionResult> Index()
        {
            var trainers = await _trainerService.GetAllTrainerAsync();
            return View(trainers);
        }
        public async Task<ActionResult> GetTrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Not Found Trainer";
                return RedirectToAction(nameof(Index));
            }
            var trainer = await _trainerService.GetTrainerByIdAsync(id);
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
        public async Task<ActionResult> CreateTrainer(CreateTrainerViewModel CreatedTrainer)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Data";
                return RedirectToAction(nameof(Create), CreatedTrainer);
            }
            bool IsTrainerCreated = await _trainerService.CreateTrainerAsync(CreatedTrainer);
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

        public async Task<ActionResult> UpdateTrainer(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Not Found Trainer";
                return RedirectToAction(nameof(Index));
            }
            var trainer = await _trainerService.GetTrainerToUpdateAsync(id);
            if (trainer is null)
            {
                TempData["Error"] = "Not Found Trainer";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateTrainer(int id, TrainerToUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Data";
                return View(viewModel);
            }
            bool IsTrainerUpdated = await _trainerService.UpdateTrainerDetailsAsync(id, viewModel);
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

        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Not Found Trainer";
                return RedirectToAction(nameof(Index));
            }
            var trainer = await _trainerService.GetTrainerByIdAsync(id);
            if (trainer is null)
            {
                TempData["Error"] = "Not Found Trainer";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            bool IsTrainerDeleted = await _trainerService.RemoveTrainerAsync(id);
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
