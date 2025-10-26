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
        IEnumerable<PlanViewModel> Index();
        PlanViewModel? GetPlanDetails(int id);
        UpdatePlanViewModel? GetPlanToUpdate(int id);
        bool UpdatePlan(int id , UpdatePlanViewModel updatedPlan);
        bool TogglePlanStatus(int id);
    }
}
