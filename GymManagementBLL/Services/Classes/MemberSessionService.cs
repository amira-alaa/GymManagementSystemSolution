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

        public async Task<bool> CreateMemberSessionAsync(CreateMemberSessionViewModel CreatedMs)
        {
            var IsMemberSessionExist = await _unitOfWork.MemberSessionRepository
                                                .GetAllAsync(ms => ms.SessionId == CreatedMs.SessionId && ms.MemberId == CreatedMs.MemberId);
                                                
            if (IsMemberSessionExist.Any()) return false;
            var MS = _mapper.Map<CreateMemberSessionViewModel, MemberSession>(CreatedMs);
            await _unitOfWork.MemberSessionRepository.AddAsync(MS);
            return await _unitOfWork.SaveChangesAsync() > 0;

        }

        public async Task<IEnumerable<MembersForDropListBookingViewModel>> GetMembersAsync()
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync();
            if (members is null || !members.Any()) return Enumerable.Empty<MembersForDropListBookingViewModel>();
            return _mapper.Map<IEnumerable<Member>, IEnumerable<MembersForDropListBookingViewModel>>(members);
        }

        public async Task<bool> DeleteMemberSessionAsync(int sessionId, int memberId)
        {
            var memberSession = await _unitOfWork.MemberSessionRepository.GetAllAsync();
            var memberSessionFirst = memberSession.FirstOrDefault(ms => ms.SessionId == sessionId && ms.MemberId == memberId);
            if (memberSessionFirst is null) return false;
            _unitOfWork.MemberSessionRepository.Delete(memberSessionFirst);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<MembersForOnGoingSessionsViewModel>> GetMembersForOnGoingsSessionsAsync(int sessionId)
        {
            var members = await _unitOfWork.MemberSessionRepository.GetMembersForOnGoingAsync(sessionId);
            if(members is null || !members.Any()) return Enumerable.Empty<MembersForOnGoingSessionsViewModel>();

            return _mapper.Map<IEnumerable<MemberSession>, IEnumerable<MembersForOnGoingSessionsViewModel>>(members);
        }

        public async Task<IEnumerable<MembersForUpComingSessionsViewModel>> GetMembersForUpComingSessionsAsync(int sessionId)
        {
            var members = await _unitOfWork.MemberSessionRepository.GetMembersForUpComingAsync(sessionId);
            if (members is null || !members.Any()) return Enumerable.Empty<MembersForUpComingSessionsViewModel>();

            return _mapper.Map<IEnumerable<MemberSession>, IEnumerable<MembersForUpComingSessionsViewModel>>(members);
        }

        public async Task<IEnumerable<MemberSessionViewModel>> GetNotCompletedMemberSessionsAsync()
        {
            var memberSessions = await _unitOfWork.MemberSessionRepository.GetNotCompletedMemberSessionsAsync();
            if(memberSessions is null || !memberSessions.Any()) return Enumerable.Empty<MemberSessionViewModel>();

            var viewModels = _mapper.Map< IEnumerable<MemberSession>,IEnumerable<MemberSessionViewModel>>(memberSessions);
            foreach(var vm in viewModels)
            {
                vm.AvailableSlots = await _unitOfWork.MemberSessionRepository.GetNumOfBookedSlotsAsync(vm.SessionId);
            }
            return viewModels;
        }

        public async Task<bool> IsAttendedAsync(int sessionId, int memberId)
        {
            var memberSession = await _unitOfWork.MemberSessionRepository.GetAllAsync();
            var memberSessionFirst = memberSession.FirstOrDefault(ms => ms.SessionId == sessionId && ms.MemberId == memberId);
            if (memberSessionFirst is null) return false;
            memberSessionFirst.IsAttended = memberSessionFirst.IsAttended ? false : true;
            _unitOfWork.MemberSessionRepository.Update(memberSessionFirst);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }


    }
}
