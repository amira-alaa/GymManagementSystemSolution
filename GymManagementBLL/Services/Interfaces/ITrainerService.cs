using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.TrainerViewModel;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainerAsync();
        Task<bool> CreateTrainerAsync(CreateTrainerViewModel createTrainer);
        Task<TrainerViewModel?> GetTrainerByIdAsync(int id);
        Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int id);
        Task<bool> UpdateTrainerDetailsAsync(int id, TrainerToUpdateViewModel trainerToUpdate);
        Task<bool> RemoveTrainerAsync(int id);
    }
}
