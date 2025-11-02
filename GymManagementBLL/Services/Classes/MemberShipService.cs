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
        public bool CreateMemberShip(CreateMemberShipViewModel memberShipViewModel)
        {
            var MemberShipisExist = GetMemberShipByIDs(memberShipViewModel.MemberId , memberShipViewModel.PlanId);
            if (MemberShipisExist != null)
                return false;
            var memberShipEntity = _mapper.Map<CreateMemberShipViewModel, MemberShip>(memberShipViewModel);
            memberShipEntity.EndDate = DateTime.Now.AddDays(3);
            _unitOfWork.MemberShipRepository.Add(memberShipEntity);
            return _unitOfWork.SaveChanges() > 0;

        }
        public MemberShip? GetMemberShipByIDs(int MemberId , int PlanId)
        {
            var membership = _unitOfWork.MemberShipRepository.GetByMemberIdAndPlanId(MemberId , PlanId);
            if (membership == null)
                return null;
            return membership;
        }
        public bool DeleteMemberShip(MemberShip memberShip)
        {
            if (memberShip == null)
                return false;
            _unitOfWork.MemberShipRepository.Delete(memberShip);
            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<SelectMemberToDropListViewModel> GetMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if(members == null || !members.Any())
                return Enumerable.Empty<SelectMemberToDropListViewModel>();
            return _mapper.Map<IEnumerable<Member>, IEnumerable<SelectMemberToDropListViewModel>>(members);
        }

        public IEnumerable<SelectPlanToDropListViewModel> GetPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans == null || !plans.Any())
                return Enumerable.Empty<SelectPlanToDropListViewModel>();
            return _mapper.Map<IEnumerable<Plan>, IEnumerable<SelectPlanToDropListViewModel>>(plans);
        }
    }
}
