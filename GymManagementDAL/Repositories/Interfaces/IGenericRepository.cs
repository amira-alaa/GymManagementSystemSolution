using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IGenericRepository<IEntity> where IEntity : BaseEntity , new()
    {
        Task<IEnumerable<IEntity>> GetAllAsync(Func<IEntity, bool>? condition = null);
        Task<IEntity?> GetByIdAsync(int Id);
        Task AddAsync(IEntity entity);
        void Update(IEntity entity);
        void Delete(IEntity entity);
    }
}
