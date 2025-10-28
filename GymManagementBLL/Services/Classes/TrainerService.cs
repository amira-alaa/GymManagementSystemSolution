using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork , IMapper mapper)
        {
           
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<TrainerViewModel> Index()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any()) return Enumerable.Empty<TrainerViewModel>();
            
            var viewModelTrainers = _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerViewModel>>(trainers);
            
            return viewModelTrainers;
        }

        public bool Create(CreateTrainerViewModel createTrainer)
        {
            try
            {

                if (IsEmailExists(createTrainer.Email) || IsPhoneExists(createTrainer.Phone)) return false;
               
                var trainer = _mapper.Map<CreateTrainerViewModel, Trainer>(createTrainer);
                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public TrainerViewModel? GetTrainer(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer == null) return null;
           
            var viewModelTrainer = _mapper.Map<Trainer, TrainerViewModel>(trainer);
            viewModelTrainer.Address = $"{trainer.Address.BulildingNumber} - {trainer.Address.Street} - {trainer.Address.City}";
            return viewModelTrainer;

        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer == null) return null;
           
            return _mapper.Map<Trainer, TrainerToUpdateViewModel>(trainer);
        }
        public bool UpdateTrainerDetails(int id, TrainerToUpdateViewModel trainerToUpdate)
        {
            try {
                var IsEmailExists = _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == trainerToUpdate.Email && x.Id != id).Any();
                var IsPhoneExists = _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Phone == trainerToUpdate.Phone && x.Id != id).Any();
                if (IsEmailExists || IsPhoneExists) return false;
                var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
                if (trainer == null) return false;
           
                _mapper.Map(trainerToUpdate, trainer);
                _unitOfWork.GetRepository<Trainer>().Update(trainer) ;
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        public bool RemoveTrainer(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer == null) return false;

            var IsHasFutureSessions = _unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == id && s.StartDate > DateTime.Now).Any();
            if (IsHasFutureSessions) return false;
            _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            return _unitOfWork.SaveChanges() > 0;

        }

        #region Helpers
        private bool IsEmailExists(string email) => _unitOfWork.GetRepository<Trainer>().GetAll(t => t.Email == email).Any();
        private bool IsPhoneExists(string phone) => _unitOfWork.GetRepository<Trainer>().GetAll(t =>t.Phone == phone).Any();
        #endregion
    }
}
