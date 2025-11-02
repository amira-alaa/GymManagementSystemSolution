using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberSessionViewModel;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class MemberSessionService : IMemberSessionService
    {
        //private readonly IMemberSessionRepository _memberSessionReposatory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberSessionService(IUnitOfWork unitOfWork , IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateMemberSession(CreateMemberSessionViewModel CreatedMs)
        {
            var IsMemberSessionExist = _unitOfWork.MemberSessionRepository
                                                .GetAll(ms => ms.SessionId == CreatedMs.SessionId && ms.MemberId == CreatedMs.MemberId)
                                                .Any();
            if (IsMemberSessionExist) return false;
            var MS = _mapper.Map<CreateMemberSessionViewModel, MemberSession>(CreatedMs);
            _unitOfWork.MemberSessionRepository.Add(MS);
            return _unitOfWork.SaveChanges() > 0;

        }

        public IEnumerable<MembersForDropListBookingViewModel> GetMembers()
        {
            var members = _unitOfWork.MemberSessionRepository.GetMembers();
            if (members is null || !members.Any()) return Enumerable.Empty<MembersForDropListBookingViewModel>();
            return _mapper.Map<IEnumerable<Member>, IEnumerable<MembersForDropListBookingViewModel>>(members);
        }

        public bool DeleteMemberSession(int sessionId, int memberId)
        {
            var memberSession = _unitOfWork.MemberSessionRepository.GetAll()
                                                       .FirstOrDefault(ms => ms.SessionId == sessionId && ms.MemberId == memberId);
            if (memberSession is null) return false;
            _unitOfWork.MemberSessionRepository.Delete(memberSession);
            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<MembersForOnGoingSessionsViewModel> GetMembersForOnGoingsSessions(int sessionId)
        {
            var members = _unitOfWork.MemberSessionRepository.GetMembersForOnGoing(sessionId);
            if(members is null || !members.Any()) return Enumerable.Empty<MembersForOnGoingSessionsViewModel>();

            return _mapper.Map<IEnumerable<MemberSession>, IEnumerable<MembersForOnGoingSessionsViewModel>>(members);
        }

        public IEnumerable<MembersForUpComingSessionsViewModel> GetMembersForUpComingSessions(int sessionId)
        {
            var members = _unitOfWork.MemberSessionRepository.GetMembersForUpComing(sessionId);
            if (members is null || !members.Any()) return Enumerable.Empty<MembersForUpComingSessionsViewModel>();

            return _mapper.Map<IEnumerable<MemberSession>, IEnumerable<MembersForUpComingSessionsViewModel>>(members);
        }

        public IEnumerable<MemberSessionViewModel> GetNotCompletedMemberSessions()
        {
            var memberSessions = _unitOfWork.MemberSessionRepository.GetNotCompletedMemberSessions();
            if(memberSessions is null || !memberSessions.Any()) return Enumerable.Empty<MemberSessionViewModel>();

            var viewModels = _mapper.Map< IEnumerable<MemberSession>,IEnumerable<MemberSessionViewModel>>(memberSessions);
            foreach(var vm in viewModels)
            {
                vm.AvailableSlots = _unitOfWork.MemberSessionRepository.GetNumOfBookedSlots(vm.SessionId);
            }
            return viewModels;
        }

        public bool IsAttended(int sessionId, int memberId)
        {
            var memberSession = _unitOfWork.MemberSessionRepository.GetAll()
                                                       .FirstOrDefault(ms => ms.SessionId == sessionId && ms.MemberId == memberId);
            if (memberSession is null) return false;
            memberSession.IsAttended = memberSession.IsAttended ? false : true;
            _unitOfWork.MemberSessionRepository.Update(memberSession);
            return _unitOfWork.SaveChanges() > 0;
        }


    }
}
