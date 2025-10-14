using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.Services.Classes
{
    internal class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public SessionViewModel? GetSessionById(int id)
        {
            var session = _unitOfWork.SessionRepository.GetSessionByIdWithTrainersAndCategories(id);
            if (session is null) return null;

            var sessionViewModel = _mapper.Map<Session, SessionViewModel>(session);
            return sessionViewModel;
        }

        public IEnumerable<SessionViewModel> Index()
        {
            var sessions = _unitOfWork.SessionRepository.GetSessionsWithTrainersAndCategories();
            var sessionsViewModels = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);

            return sessionsViewModels;

        }


        public bool CreateSession(CreateSessionViewModel session)
        {
            try
            {
                if (!IsTrainerExists(session.TrainerId) && !IsCategoryExists(session.CategoryId) && !IsValidSessionDates(session.StartDate, session.EndDate))
                    return false;

                var Session = _mapper.Map<CreateSessionViewModel, Session>(session);
                _unitOfWork.SessionRepository.Add(Session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
               return false;
            }
        }
        public UpdateSessionViewModel? GetSessionForUpdate(int id)
        {
            var session = _unitOfWork.SessionRepository.GetById(id);
            if (!IsSessionAvailableToUpdate(session!)) return null;
            return _mapper.Map<Session, UpdateSessionViewModel>(session!);

        }

        public bool UpdateSession(UpdateSessionViewModel UpdateSession , int Id)
        {
            var session = _unitOfWork.SessionRepository.GetById(Id);
            if (!IsSessionAvailableToUpdate(session!)) return false;
            if (!IsTrainerExists(UpdateSession.TrainerId) && !IsValidSessionDates(UpdateSession.StartDate, UpdateSession.EndDate))
                return false;
            _mapper.Map(UpdateSession, session);
            _unitOfWork.SessionRepository.Update(session!);
            return _unitOfWork.SaveChanges() > 0;
        }
        public bool RemoveSession(int id)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(id);
                if (!IsSessionAvailableToRemove(session!)) return false;
                _unitOfWork.SessionRepository.Delete(session!);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch
            {
               return false;
            }
        }

        #region Helpers
        private bool IsTrainerExists(int trainerId) => _unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;
        private bool IsCategoryExists(int categoryId) => _unitOfWork.GetRepository<Category>().GetById(categoryId) is not null;
        private bool IsValidSessionDates(DateTime startDate, DateTime endDate) => startDate < endDate && startDate > DateTime.Now;

        private bool IsSessionAvailableToUpdate(Session session)
        {
            if (session is null) return false;
            if (session.StartDate <= DateTime.Now) return false;
            if (session.EndDate < DateTime.Now) return false;
            var HasActiveBookings = _unitOfWork.SessionRepository.GetNumOfBookedSlots(session.Id) > 0;
            if (HasActiveBookings) return false;
            return true;
        }

        private bool IsSessionAvailableToRemove(Session session)
        {
            if (session is null) return false;
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;
            if (session.StartDate > DateTime.Now) return false;
            var HasActiveBookings = _unitOfWork.SessionRepository.GetNumOfBookedSlots(session.Id) > 0;
            if (HasActiveBookings) return false;
            return true;
        }


        #endregion
    }
}
