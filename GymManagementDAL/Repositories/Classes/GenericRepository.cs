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
        public async Task AddAsync(IEntity entity) => await _dbContext.Set<IEntity>().AddAsync(entity);

        public void Delete(IEntity entity)=> _dbContext.Set<IEntity>().Remove(entity);


        public async Task<IEnumerable<IEntity>> GetAllAsync(Func<IEntity, bool>? condition = null)
        {
            if (condition is null) return await _dbContext.Set<IEntity>().AsNoTracking().ToListAsync();
            else return _dbContext.Set<IEntity>().AsNoTracking().Where(condition).ToList();

        }

        public async Task<IEntity?> GetByIdAsync(int Id) => await _dbContext.Set<IEntity>().FindAsync(Id);

        public void Update(IEntity entity)=> _dbContext.Set<IEntity>().Update(entity);
      
    }
}
