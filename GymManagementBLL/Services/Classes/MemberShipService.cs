using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberShipViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class MemberShipService : IMemberShipService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MemberShipService(IMapper mapper , IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<MemberShipViewModel> GetAllMemberShips()
        {
            var memberShips = _unitOfWork.MemberShipRepository.GetMemberShips();
            if (memberShips == null || !memberShips.Any())
                return Enumerable.Empty<MemberShipViewModel>();
            return _mapper.Map<IEnumerable<MemberShip>, IEnumerable<MemberShipViewModel>>(memberShips);
        }
        public async Task<bool> CreateMemberShipAsync(CreateMemberShipViewModel memberShipViewModel)
        {
            var MemberShipisExist = await GetMemberShipByIDsAsync(memberShipViewModel.MemberId , memberShipViewModel.PlanId);
            if (MemberShipisExist != null)
                return false;
            var memberShipEntity = _mapper.Map<CreateMemberShipViewModel, MemberShip>(memberShipViewModel);
            memberShipEntity.EndDate = DateTime.Now.AddDays(3);
            await _unitOfWork.MemberShipRepository.AddAsync(memberShipEntity);
            return await _unitOfWork.SaveChangesAsync() > 0;

        }
        public async Task<MemberShip?> GetMemberShipByIDsAsync(int MemberId , int PlanId)
        {
            var membership = await _unitOfWork.MemberShipRepository.GetByMemberIdAndPlanIdAsync(MemberId , PlanId);
            if (membership == null)
                return null;
            return membership;
        }
        public async Task<bool> DeleteMemberShipAsync(MemberShip memberShip)
        {
            if (memberShip == null)
                return false;
            _unitOfWork.MemberShipRepository.Delete(memberShip);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<SelectMemberToDropListViewModel>> GetMembersAsync()
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync();
            if(members == null || !members.Any())
                return Enumerable.Empty<SelectMemberToDropListViewModel>();
            return _mapper.Map<IEnumerable<Member>, IEnumerable<SelectMemberToDropListViewModel>>(members);
        }

        public async Task<IEnumerable<SelectPlanToDropListViewModel>> GetPlansAsync()
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync();
            if (plans == null || !plans.Any())
                return Enumerable.Empty<SelectPlanToDropListViewModel>();
            return _mapper.Map<IEnumerable<Plan>, IEnumerable<SelectPlanToDropListViewModel>>(plans);
        }
    }
}
