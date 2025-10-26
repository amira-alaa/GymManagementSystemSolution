using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Execution;
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

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<MemberViewModel> Index()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if(members is null || !members.Any() ) return Enumerable.Empty<MemberViewModel>();
            #region Way01
            //var viewModelMembers = new List<MemberViewModel>();
            //foreach(var member in members)
            //{
            //    var viewModelMember = new MemberViewModel
            //    {
            //        Id = member.Id,
            //        Name = member.Name,
            //        Email = member.Email,
            //        Phone = member.Phone,
            //        Gender = member.Gender.ToString(),
            //        Photo = member.Photo
            //    };
            //    viewModelMembers.Add(viewModelMember);
            //}
            #endregion

            #region Way02
            //var viewModelMembers = members.Select(m => new MemberViewModel
            //{
            //    Id = m.Id,
            //    Name = m.Name,
            //    Email = m.Email,
            //    Phone = m.Phone,
            //    Gender = m.Gender.ToString(),
            //    Photo = m.Photo
            //});
            #endregion

            var viewModelMembers = _mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModel>>(members);
            return viewModelMembers;
        }
        public MemberViewModel? GetMember(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member == null) return null;
           
            var viewModelMember = _mapper.Map< Member, MemberViewModel>(member);
            var ActiveMemberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(m => m.MemberId == id && m.Status == "Active").FirstOrDefault();
            if(ActiveMemberShip is not null)
            {
                viewModelMember.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                viewModelMember.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
                viewModelMember.PlanName = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId)?.Name;
            }
            return viewModelMember;
        }

        public bool Create(CreateMemberViewModel createMember)
        {
            try
            {
                
                if (IsEmailExist(createMember.Email) || IsPhoneExist(createMember.Phone)) return false;
               
                var member = _mapper.Map<CreateMemberViewModel, Member>(createMember);
              
                _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public HealthRecordViewModel? GetHealthRecord(int id)
        {
            var healthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(id);
            if (healthRecord == null) return null;
           
            var healthRecordViewModel = _mapper.Map<HealthRecord, HealthRecordViewModel>(healthRecord);
            return healthRecordViewModel;
        }


        public MemberToUpdateViewModel? GetMemberToUpdate(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member == null) return null;
            return _mapper.Map<Member, MemberToUpdateViewModel>(member);
           
         
        }

        public bool UpdateMemberDetails(int id, MemberToUpdateViewModel memberToUpdate)
        {
            try
            {
                var IsEmailExists = _unitOfWork.GetRepository<Member>().GetAll(x => x.Email == memberToUpdate.Email && x.Id != id).Any();
                var IsPhoneExists = _unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == memberToUpdate.Phone && x.Id != id).Any();

                if (IsEmailExists || IsPhoneExists ) return false;
                var member = _unitOfWork.GetRepository<Member>().GetById(id);
                if (member == null) return false;
                //member.Name = memberToUpdate.Name;
                //member.Email = memberToUpdate.Email;
                //member.Phone = memberToUpdate.Phone;
                //member.Address.BulildingNumber = memberToUpdate.BuildingNumber;
                //member.Address.Street = memberToUpdate.Street;
                //member.Address.City = memberToUpdate.City;
                //member.Photo = memberToUpdate.Photo;
                _mapper.Map(memberToUpdate, member);
                _unitOfWork.GetRepository<Member>().Update(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        public bool RemoveMember(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member is null) return false;

            var SessionIds = _unitOfWork.GetRepository<MemberSession>().GetAll(x => x.MemberId == id).Select(x => x.SessionId);
            var IsActiveSessions = _unitOfWork.GetRepository<Session>().GetAll(x => SessionIds.Contains(x.Id) && x.StartDate > DateTime.Now).Any();
            if(IsActiveSessions) return false;



            var memberships = _unitOfWork.GetRepository<MemberShip>().GetAll(x => x.Id == member.Id);
            if (memberships.Any())
            {
                foreach (var item in memberships)
                {
                    _unitOfWork.GetRepository<MemberShip>().Delete(item);
                }

            }
            _unitOfWork.GetRepository<Member>().Delete(member);
            return _unitOfWork.SaveChanges() > 0;

        }





        #region Helpers

        private bool IsEmailExist(string email) => _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();
        private bool IsPhoneExist(string phone) => _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();
        #endregion
    }
}
