using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Execution;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticesViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;

using Member = GymManagementDAL.Entities.Member;

namespace GymManagementBLL.Services.Classes
{
    public class AnalyticesService : IAnalyticesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public AnalyticesViewModel GetAnalyticesData()
        {
            return new AnalyticesViewModel()
            {
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                ActiveMembers = _unitOfWork.GetRepository<MemberShip>().GetAll(x => x.Status == "Active").Count(),
                Trainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpComingSessions = _unitOfWork.GetRepository<Session>().GetAll(x => x.StartDate > DateTime.Now).Count(),
                OnGoingSessions = _unitOfWork.GetRepository<Session>().GetAll(x => x.StartDate <= DateTime.Now && x.EndDate > DateTime.Now).Count(),
                CompletedSessions = _unitOfWork.GetRepository<Session>().GetAll(x => x.EndDate <= DateTime.Now).Count()
            };
        }

    }
}
