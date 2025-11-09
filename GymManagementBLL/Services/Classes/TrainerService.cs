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
        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainerAsync()
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync();
            if (trainers is null || !trainers.Any()) return Enumerable.Empty<TrainerViewModel>();
            
            var viewModelTrainers = _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerViewModel>>(trainers);
            
            return viewModelTrainers;
        }

        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var IsEmailExists = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(x => x.Email == createTrainer.Email);
                var IsPhoneExists = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(x => x.Phone == createTrainer.Phone);
                if (IsEmailExists.Any() || IsPhoneExists.Any()) return false;
               
                var trainer = _mapper.Map<CreateTrainerViewModel, Trainer>(createTrainer);
                await _unitOfWork.GetRepository<Trainer>().AddAsync(trainer);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<TrainerViewModel?> GetTrainerByIdAsync(int id)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id);
            if (trainer == null) return null;
           
            var viewModelTrainer = _mapper.Map<Trainer, TrainerViewModel>(trainer);
            viewModelTrainer.Address = $"{trainer.Address.BulildingNumber} - {trainer.Address.Street} - {trainer.Address.City}";
            return viewModelTrainer;

        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int id)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id);
            if (trainer == null) return null;
           
            return _mapper.Map<Trainer, TrainerToUpdateViewModel>(trainer);
        }
        public async Task<bool> UpdateTrainerDetailsAsync(int id, TrainerToUpdateViewModel trainerToUpdate)
        {
            try {
                var IsEmailExists = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(x => x.Email == trainerToUpdate.Email && x.Id != id);
                var IsPhoneExists = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(x => x.Phone == trainerToUpdate.Phone && x.Id != id);
                if (IsEmailExists.Any() || IsPhoneExists.Any()) return false;
                var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id);
                if (trainer == null) return false;
           
                _mapper.Map(trainerToUpdate, trainer);
                _unitOfWork.GetRepository<Trainer>().Update(trainer) ;
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> RemoveTrainerAsync(int id)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id);
            if (trainer == null) return false;

            var IsHasFutureSessions = await _unitOfWork.GetRepository<Session>().GetAllAsync(s => s.TrainerId == id && s.StartDate > DateTime.Now);
            if (IsHasFutureSessions.Any()) return false;
            _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            return await _unitOfWork.SaveChangesAsync() > 0;

        }

       
    }
}
