using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<PlanViewModel> Index()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans == null || !plans.Any()) return Enumerable.Empty<PlanViewModel>();
            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanViewModel>>(plans);
            
        }
        public PlanViewModel? GetPlanDetails(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan == null) return null;
            return _mapper.Map<Plan, PlanViewModel>(plan);
            
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan == null || plan.IsActive == false || IsPlanActive(id)) return null;
            return _mapper.Map<Plan, UpdatePlanViewModel>(plan);
           
        }



        public bool UpdatePlan(int id, UpdatePlanViewModel updatedPlan)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);

            if (plan == null || IsPlanActive(id)) return false;

            _mapper.Map(updatedPlan, plan);
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
