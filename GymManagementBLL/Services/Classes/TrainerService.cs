using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    internal class TrainerService : ITrainerService
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
           
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<TrainerViewModel> Index()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any()) return Enumerable.Empty<TrainerViewModel>();
            var viewModelTrainers = trainers.Select(t => new TrainerViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialties = t.Specialties.ToString()
            });
            return viewModelTrainers;
        }

        public bool Create(CreateTrainerViewModel createTrainer)
        {
            try
            {
                if (IsEmailExists(createTrainer.Email) || IsPhoneExists(createTrainer.Phone)) return false;
                var trainer = new Trainer()
                {
                    Name = createTrainer.Name,
                    Email = createTrainer.Email,
                    Phone = createTrainer.Phone,
                    DateOfBirth = createTrainer.BirthDate,
                    Gender = createTrainer.Gender,
                    Address = new Address()
                    {
                        BulildingNumber = createTrainer.BuildingNumber,
                        Street = createTrainer.Street,
                        City = createTrainer.City
                    },
                    Specialties = createTrainer.specialties
                };
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
            return new TrainerViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialties = trainer.Specialties.ToString(),
                Address = $"{trainer.Address.BulildingNumber} - {trainer.Address.Street} - {trainer.Address.City}"
            };

        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int id)
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainers == null) return null;
            return new TrainerToUpdateViewModel()
            {
                Name = trainers.Name,
                Email = trainers.Email,
                Phone = trainers.Phone,
                BuildingNumber = trainers.Address.BulildingNumber,
                Street = trainers.Address.Street,
                City = trainers.Address.City,
                specialties = trainers.Specialties
            };
        }
        public bool UpdateTrainerDetails(int id, TrainerToUpdateViewModel trainerToUpdate)
        {
            try {
                if (IsEmailExists(trainerToUpdate.Email) || IsPhoneExists(trainerToUpdate.Phone)) return false;
                var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
                if (trainer == null) return false;
                trainer.Name = trainerToUpdate.Name;
                trainer.Email = trainerToUpdate.Email;
                trainer.Phone = trainerToUpdate.Phone;
                trainer.Address.BulildingNumber = trainerToUpdate.BuildingNumber;
                trainer.Address.Street = trainerToUpdate.Street;
                trainer.Address.City = trainerToUpdate.City;
                trainer.Specialties = trainerToUpdate.specialties;
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

        private bool IsEmailExists(string email) => _unitOfWork.GetRepository<Trainer>().GetAll(t => t.Email == email).Any();
        private bool IsPhoneExists(string phone) => _unitOfWork.GetRepository<Trainer>().GetAll(t =>t.Phone == phone).Any();

    }
}
