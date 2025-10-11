using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.TrainerViewModel;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface ITrainerService
    {
        IEnumerable<TrainerViewModel> Index();
        bool Create(CreateTrainerViewModel createTrainer);
        TrainerViewModel? GetTrainer(int id);
        TrainerToUpdateViewModel? GetTrainerToUpdate(int id);
        bool UpdateTrainerDetails(int id, TrainerToUpdateViewModel trainerToUpdate);
        bool RemoveTrainer(int id);
    }
}
