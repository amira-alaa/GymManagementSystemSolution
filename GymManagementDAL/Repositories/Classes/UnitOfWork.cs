using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;

        public UnitOfWork(GymDbContext dbContext, ISessionRepository sessionRepository ,
            IMemberSessionRepository memberSessionRepository, IMemberShipRepository memberShipRepository)
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
            MemberSessionRepository = memberSessionRepository;
            MemberShipRepository = memberShipRepository;
        }
        private Dictionary<Type, object> _repositories = new();

        public ISessionRepository SessionRepository { get; }

        public IMemberSessionRepository MemberSessionRepository { get; }

        public IMemberShipRepository MemberShipRepository { get; }

        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity, new()
        {
            var EntityType = typeof(T);
            if (_repositories.ContainsKey(EntityType))
                return (IGenericRepository<T>)_repositories[EntityType];
            var newRepository = new GenericRepository<T>(_dbContext);
            _repositories[EntityType] = newRepository;
            return newRepository;
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
