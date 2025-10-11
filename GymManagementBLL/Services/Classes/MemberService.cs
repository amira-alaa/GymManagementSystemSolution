using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    internal class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;


        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            var viewModelMembers = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Gender = m.Gender.ToString(),
                Photo = m.Photo
            });
            #endregion
            return viewModelMembers;
        }

        public bool Create(CreateMemberViewModel createMember)
        {
            try
            {
                
                if (IsEmailExist(createMember.Email) || IsPhoneExist(createMember.Phone)) return false;
                var member = new Member()
                {
                    Name = createMember.Name,
                    Email = createMember.Email,
                    Phone = createMember.Phone,
                    DateOfBirth = createMember.BirthDate,
                    Gender = createMember.Gender,
                    Address = new Address()
                    {
                        BulildingNumber = createMember.BuildingNumber,
                        Street = createMember.Street,
                        City = createMember.City
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createMember.HealthRecord.Height,
                        Weight = createMember.HealthRecord.Weight,
                        BloodType = createMember.HealthRecord.BloodType,
                        Note = createMember.HealthRecord.Note
                    }
                };
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
            var healthRecords = _unitOfWork.GetRepository<HealthRecord>().GetById(id);
            if (healthRecords == null) return null;
            var healthRecordsViewModel = new HealthRecordViewModel()
            {
                Height = healthRecords.Height,
                Weight = healthRecords.Weight,
                BloodType = healthRecords.BloodType,
                Note = healthRecords.Note
            };
            return healthRecordsViewModel;
        }

        public MemberViewModel? GetMember(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member == null) return null;
            var viewModelMember = new MemberViewModel
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender.ToString(),
                Photo = member.Photo,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address?.BulildingNumber}, {member.Address?.Street}, {member.Address?.City}",

            };
            var ActiveMemberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(m => m.MemberId == id && m.Status == "Active").FirstOrDefault();
            if(ActiveMemberShip is not null)
            {
                viewModelMember.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                viewModelMember.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
                viewModelMember.PlanName = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId)?.Name;
            }
            return viewModelMember;
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member == null) return null;
            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.Address.BulildingNumber,
                Street = member.Address.Street,
                City = member.Address.City,
                Photo = member.Photo
            };
         
        }

        public bool UpdateMemberDetails(int id, MemberToUpdateViewModel memberToUpdate)
        {
            try
            {
                if (IsEmailExist(memberToUpdate.Email) || IsPhoneExist(memberToUpdate.Phone)) return false;
                var member = _unitOfWork.GetRepository<Member>().GetById(id);
                if (member == null) return false;
                member.Name = memberToUpdate.Name;
                member.Email = memberToUpdate.Email;
                member.Phone = memberToUpdate.Phone;
                member.Address.BulildingNumber = memberToUpdate.BuildingNumber;
                member.Address.Street = memberToUpdate.Street;
                member.Address.City = memberToUpdate.City;
                member.Photo = memberToUpdate.Photo;
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
            var IsActiveSessions = _unitOfWork.GetRepository<MemberSession>().GetAll(x => x.Id == id && x.Session.StartDate > DateTime.Now).Any();
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







        private bool IsEmailExist(string email) => _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();
        private bool IsPhoneExist(string phone) => _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();

    }
}
