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

        public async Task<AnalyticesViewModel> GetAnalyticesDataAsync()
        {
            var totalMembers = await _unitOfWork.GetRepository<Member>().GetAllAsync();
            var activeMembers = await _unitOfWork.GetRepository<MemberShip>().GetAllAsync(x => x.Status == "Active");
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync();
            var upComingSessions = await _unitOfWork.GetRepository<Session>().GetAllAsync(x => x.StartDate > DateTime.Now);
            var onGoingSessions = await _unitOfWork.GetRepository<Session>().GetAllAsync(x => x.StartDate <= DateTime.Now && x.EndDate > DateTime.Now);
            var completedSessions = await _unitOfWork.GetRepository<Session>().GetAllAsync(x => x.EndDate <= DateTime.Now);
            return new AnalyticesViewModel()
            {
                TotalMembers = totalMembers.Count(),
                ActiveMembers = activeMembers.Count(),
                Trainers = trainers.Count(),
                UpComingSessions = upComingSessions.Count(),
                OnGoingSessions = onGoingSessions.Count(),
                CompletedSessions = completedSessions.Count()
            };
        }

    }
}
