using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    internal class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<PlanViewModel> Index()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans == null || !plans.Any()) return Enumerable.Empty<PlanViewModel>();
            return plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                Price = p.Price,
                IsActive = p.IsActive
            });
        }
        public PlanViewModel? GetPlanDetails(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan == null) return null;
            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
        }

        public PlanViewModel? GetPlanToUpdate(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan == null || plan.IsActive == false || IsPlanActive(id)) return null;
            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
        }



        public bool UpdatePlan(int id, UpdatePlanViewModel updatedPlan)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);

            if (plan == null || IsPlanActive(id)) return false;

            plan.Name = updatedPlan.Name;
            plan.Description = updatedPlan.Description;
            plan.Price = updatedPlan.Price;
            plan.DurationDays = updatedPlan.DurationDays;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            return _unitOfWork.SaveChanges() > 0;

        }
        public bool TogglePlanStatus(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);

            if (plan == null || IsPlanActive(id)) return false;
            plan.IsActive = plan.IsActive == true ? false : true;

            try
            {
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }
        }

        #region Helper Methods
        private bool IsPlanActive(int id) => _unitOfWork.GetRepository<MemberShip>().GetAll(x => x.PlanId == id && x.Status == "Active").Any();
        #endregion
    }
}
