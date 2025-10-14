using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Member
            CreateMap<Member, MemberViewModel>();

            CreateMap<CreateMemberViewModel, Member>()
                     .ForMember(m => m.Address.BulildingNumber, options => options.MapFrom(vm => vm.BuildingNumber));

            CreateMap<HealthRecord, HealthRecordViewModel>();
            CreateMap<Member, MemberToUpdateViewModel>().ReverseMap()
                .ForMember(m => m.Address.BulildingNumber, options => options.MapFrom(vm => vm.BuildingNumber));
            #endregion

            #region Trainer
            CreateMap<Trainer, TrainerViewModel>();

            CreateMap<CreateTrainerViewModel, Trainer>()
                     .ForMember(t => t.Address.BulildingNumber, options => options.MapFrom(vm => vm.BuildingNumber));

            CreateMap<Member, MemberToUpdateViewModel>().ReverseMap()
                .ForMember(m => m.Address.BulildingNumber, options => options.MapFrom(vm => vm.BuildingNumber));
            #endregion

            #region Plan
            CreateMap<Plan, PlanViewModel>();
            CreateMap<UpdatePlanViewModel , Plan>().ReverseMap();
               
            #endregion
            #region Session
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.SessionCategory.CategoryName))
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.SessionTrainer.Name))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session , UpdateSessionViewModel>().ReverseMap();
            #endregion
        }
    }
}
