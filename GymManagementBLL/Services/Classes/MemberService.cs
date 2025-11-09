using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Execution;
using GymManagementBLL.Services.AttachmentService;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Member = GymManagementDAL.Entities.Member;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper , IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync()
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync();
            if(members is null || !members.Any() ) return Enumerable.Empty<MemberViewModel>();
         
            var viewModelMembers = _mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModel>>(members);
            return viewModelMembers;
        }
        public async Task<MemberViewModel?> GetMemberByIdAsync(int id)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id);
            if (member == null) return null;
           
            var viewModelMember = _mapper.Map< Member, MemberViewModel>(member);
            var activeMemberShip = await _unitOfWork.GetRepository<MemberShip>()
                                                    .GetAllAsync(m => m.MemberId == id && m.Status == "Active");
            var ActiveMemberShip = activeMemberShip.FirstOrDefault();
            if (ActiveMemberShip is not null)
            {
                viewModelMember.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                viewModelMember.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
                var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(ActiveMemberShip.PlanId);
                viewModelMember.PlanName = plan?.Name;
            }
            return viewModelMember;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel createMember)
        {
            try
            {
                var IsEmailExists =await _unitOfWork.GetRepository<Member>().GetAllAsync(m => m.Email == createMember.Email);
                var IsPhoneExists = await _unitOfWork.GetRepository<Member>().GetAllAsync(m => m.Phone == createMember.Phone);
                
                if (IsEmailExists.Any() || IsPhoneExists.Any()) return false;

                var PhotoName = _attachmentService.Upload("Members", createMember.PhotoFile);
                if(string.IsNullOrEmpty(PhotoName)) return false;
               
                var member = _mapper.Map<CreateMemberViewModel, Member>(createMember);
                member.Photo = PhotoName;

                await _unitOfWork.GetRepository<Member>().AddAsync(member);
                bool IsCreated =await _unitOfWork.SaveChangesAsync() > 0;
                if (!IsCreated)
                {
                    _attachmentService.Delete("Members", PhotoName);
                }
                return IsCreated;
            }
            catch
            {
                return false;
            }
        }

        public async Task<HealthRecordViewModel?> GetHealthRecordAsync(int id)
        {
            var healthRecord = await _unitOfWork.GetRepository<HealthRecord>().GetByIdAsync(id);
            if (healthRecord == null) return null;
           
            var healthRecordViewModel = _mapper.Map<HealthRecord, HealthRecordViewModel>(healthRecord);
            return healthRecordViewModel;
        }
        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id);
            if (member == null) return null;
            return _mapper.Map<Member, MemberToUpdateViewModel>(member);  
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel memberToUpdate)
        {
            try
            {
                var IsEmailExists = await _unitOfWork.GetRepository<Member>().GetAllAsync(x => x.Email == memberToUpdate.Email && x.Id != id);
                var IsPhoneExists = await _unitOfWork.GetRepository<Member>().GetAllAsync(x => x.Phone == memberToUpdate.Phone && x.Id != id);

                if (IsEmailExists.Any() || IsPhoneExists.Any() ) return false;
                var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id);
                if (member == null) return false;
             
                _mapper.Map(memberToUpdate, member);
                _unitOfWork.GetRepository<Member>().Update(member);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> RemoveMemberAsync(int id)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id);
            if (member is null) return false;

            var SessionIds = await _unitOfWork.GetRepository<MemberSession>().GetAllAsync(x => x.MemberId == id);
            var SessionIdsList = SessionIds.Select(x => x.SessionId);

            var IsActiveSessions = await _unitOfWork.GetRepository<Session>().GetAllAsync(x => SessionIdsList.Contains(x.Id) && x.StartDate > DateTime.Now);
            
            if(IsActiveSessions.Any()) return false;

            var memberships = await _unitOfWork.GetRepository<MemberShip>().GetAllAsync(x => x.Id == member.Id);
            if (memberships.Any())
            {
                foreach (var item in memberships)
                {
                    _unitOfWork.GetRepository<MemberShip>().Delete(item);
                }

            }
            _unitOfWork.GetRepository<Member>().Delete(member);
            bool IsDeleted = await _unitOfWork.SaveChangesAsync() > 0;
            if (IsDeleted)
            {
                _attachmentService.Delete("Members", member.Photo);
            }
            return IsDeleted;

        }

    }
}
