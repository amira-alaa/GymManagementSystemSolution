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

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync()
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync();
            if (plans == null || !plans.Any()) return Enumerable.Empty<PlanViewModel>();
            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanViewModel>>(plans);
            
        }
        public async Task<PlanViewModel?> GetPlanDetailsAsync(int id)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id);
            if (plan == null) return null;
            return _mapper.Map<Plan, PlanViewModel>(plan);
            
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id);
            if (plan == null || plan.IsActive == false || await IsPlanActiveAsync(id)) return null;
            return _mapper.Map<Plan, UpdatePlanViewModel>(plan);
           
        }

        public async Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel updatedPlan)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id);

            if (plan == null || await IsPlanActiveAsync(id)) return false;

            _mapper.Map(updatedPlan, plan);
            _unitOfWork.GetRepository<Plan>().Update(plan);
            return await _unitOfWork.SaveChangesAsync() > 0;

        }
        public async Task<bool> TogglePlanStatusAsync(int id)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id);

            if (plan == null || await IsPlanActiveAsync(id)) return false;
            plan.IsActive = plan.IsActive == true ? false : true;

            try
            {
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return await _unitOfWork.SaveChangesAsync() > 0;

            }
            catch
            {
                return false;
            }
        }

        #region Helper Methods
        private async Task<bool> IsPlanActiveAsync(int id)
        {
            var plans =await  _unitOfWork.GetRepository<MemberShip>().GetAllAsync(x => x.PlanId == id && x.Status == "Active");
            return plans.Any();
        }
        #endregion
    }
}
