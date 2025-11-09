using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<SessionViewModel?> GetSessionByIdAsync(int id)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionByIdWithTrainersAndCategoriesAsync(id);
            if (session is null) return null;

            var sessionViewModel = _mapper.Map<Session, SessionViewModel>(session);
            return sessionViewModel;
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync()
        {
            var sessions = await _unitOfWork.SessionRepository.GetSessionsWithTrainersAndCategoriesAsync();
            var sessionsViewModels = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);

            return sessionsViewModels;

        }
        public async Task<bool> CreateSessionAsync(CreateSessionViewModel session)
        {
            try
            {
                if (!IsTrainerExists(session.TrainerId) && !IsCategoryExists(session.CategoryId) && !IsValidSessionDates(session.StartDate, session.EndDate))
                    return false;

                var Session = _mapper.Map<CreateSessionViewModel, Session>(session);
                await _unitOfWork.SessionRepository.AddAsync(Session);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
               return false;
            }
        }

        public async Task<UpdateSessionViewModel?> GetSessionForUpdateAsync(int id)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(id);
            if (!await IsSessionAvailableToUpdateAsync(session!)) return null;
            return _mapper.Map<Session, UpdateSessionViewModel>(session!);

        }

        public async Task<bool> UpdateSessionAsync(UpdateSessionViewModel UpdateSession , int Id)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(Id);
            if (!await IsSessionAvailableToUpdateAsync(session!)) return false;
            if (!IsTrainerExists(UpdateSession.TrainerId) && !IsValidSessionDates(UpdateSession.StartDate, UpdateSession.EndDate))
                return false;
            _mapper.Map(UpdateSession, session);
            _unitOfWork.SessionRepository.Update(session!);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<bool> RemoveSessionAsync(int id)
        {
            try
            {
                var session = await _unitOfWork.SessionRepository.GetByIdAsync(id);
                if (!await IsSessionAvailableToRemoveAsync(session!)) return false;
                _unitOfWork.SessionRepository.Delete(session!);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
               return false;
            }
        }
        public async Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainerForDropDownAsync()
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetAllCategoryForDropDownAsync()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        #region Helpers
        private bool IsTrainerExists(int trainerId) => _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId) is not null;
        private bool IsCategoryExists(int categoryId) => _unitOfWork.GetRepository<Category>().GetByIdAsync(categoryId) is not null;
        private bool IsValidSessionDates(DateTime startDate, DateTime endDate) => startDate < endDate && startDate > DateTime.Now;

        private async Task<bool> IsSessionAvailableToUpdateAsync(Session session)
        {
            if (session is null) return false;
            if (session.StartDate <= DateTime.Now) return false; // ongoing
            if (session.EndDate < DateTime.Now) return false;    // completed
            var HasActiveBookings = await _unitOfWork.SessionRepository.GetNumOfBookedSlotsAsync(session.Id) > 0;
            if (HasActiveBookings) return false;
            return true;
        }

        private async Task<bool> IsSessionAvailableToRemoveAsync(Session session)
        {
            if (session is null) return false;
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;
            if (session.StartDate > DateTime.Now) return false;
            var HasActiveBookings = await _unitOfWork.SessionRepository.GetNumOfBookedSlotsAsync(session.Id) > 0;
            if (HasActiveBookings) return false;
            return true;
        }




        #endregion
    }
}
