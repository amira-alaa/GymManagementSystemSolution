using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL
{

    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            MapTrainer();
            MapSession();
            MapMember();
            MapPlan();
        }

        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BulildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));
            CreateMap<Trainer, TrainerViewModel>();
            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dist => dist.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dist => dist.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dist => dist.BuildingNumber, opt => opt.MapFrom(src => src.Address.BulildingNumber));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Address.BulildingNumber = src.BuildingNumber;
                dest.Address.City = src.City;
                dest.Address.Street = src.Street;
                dest.UpdatedAt = DateTime.Now;
            });
        }
        private void MapSession()
        {
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, SessionViewModel>()
                        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.SessionCategory.CategoryName))
                        .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.SessionTrainer.Name))
                        .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore()); 
            CreateMap<UpdateSessionViewModel, Session>().ReverseMap();


            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dist => dist.Name, opt => opt.MapFrom(src => src.CategoryName));
        }
        private void MapMember()
        {
            CreateMap<CreateMemberViewModel, Member>()
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                    {
                        BulildingNumber = src.BuildingNumber,
                        City = src.City,
                        Street = src.Street
                    })).ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecord));


            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();
            CreateMap<Member, MemberViewModel>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BulildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<Member, MemberToUpdateViewModel>()
            .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BulildingNumber))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BulildingNumber = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    dest.UpdatedAt = DateTime.Now;
                });
        }
        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<UpdatePlanViewModel, Plan>()
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        }

    //public MappingProfile()
    //    {
    //        #region Member
    //        CreateMap<Member, MemberViewModel>();

    //        CreateMap<CreateMemberViewModel, Member>()
    //                 .ForMember(m => m.Address.BulildingNumber, options => options.MapFrom(vm => vm.BuildingNumber));

    //        CreateMap<HealthRecord, HealthRecordViewModel>();
    //        CreateMap<Member, MemberToUpdateViewModel>().ReverseMap()
    //            .ForMember(m => m.Address.BulildingNumber, options => options.MapFrom(vm => vm.BuildingNumber));
    //        #endregion

    //        #region Trainer
    //        CreateMap<Trainer, TrainerViewModel>();

    //        CreateMap<CreateTrainerViewModel, Trainer>()
    //                 .ForMember(t => t.Address.BulildingNumber, options => options.MapFrom(vm => vm.BuildingNumber));

    //        CreateMap<Member, MemberToUpdateViewModel>().ReverseMap()
    //            .ForMember(m => m.Address.BulildingNumber, options => options.MapFrom(vm => vm.BuildingNumber));
    //        #endregion

    //        #region Plan
    //        CreateMap<Plan, PlanViewModel>();
    //        CreateMap<UpdatePlanViewModel , Plan>().ReverseMap();
               
    //        #endregion
    //        #region Session
    //        CreateMap<Session, SessionViewModel>()
    //            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.SessionCategory.CategoryName))
    //            .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.SessionTrainer.Name))
    //            .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());

    //        CreateMap<CreateSessionViewModel, Session>();
    //        CreateMap<Session , UpdateSessionViewModel>().ReverseMap();
    //        #endregion
    //    }
    }
}
