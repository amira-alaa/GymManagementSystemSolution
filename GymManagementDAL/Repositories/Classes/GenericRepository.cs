using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class GenericRepository<IEntity> : IGenericRepository<IEntity> where IEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;

        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(IEntity entity) => _dbContext.Set<IEntity>().Add(entity);

        public void Delete(IEntity entity)=> _dbContext.Set<IEntity>().Remove(entity);


        public IEnumerable<IEntity> GetAll(Func<IEntity, bool>? condition = null)
        {
            if (condition is null) return _dbContext.Set<IEntity>().AsNoTracking().ToList();
            else return _dbContext.Set<IEntity>().AsNoTracking().Where(condition).ToList();

        }

        public IEntity? GetById(int Id) => _dbContext.Set<IEntity>().Find(Id);

        public void Update(IEntity entity)=> _dbContext.Set<IEntity>().Update(entity);
      
    }
}
