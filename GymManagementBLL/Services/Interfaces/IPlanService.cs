using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.PlanViewModel;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync();
        Task<PlanViewModel?> GetPlanDetailsAsync(int id);
        Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id);
        Task<bool> UpdatePlanAsync(int id , UpdatePlanViewModel updatedPlan);
        Task<bool> TogglePlanStatusAsync(int id);
    }
}
