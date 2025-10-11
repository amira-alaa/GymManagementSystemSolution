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
        IEnumerable<IEntity> GetAll(Func<IEntity, bool>? condition = null);
        IEntity? GetById(int Id);
        void Add(IEntity entity);
        void Update(IEntity entity);
        void Delete(IEntity entity);
    }
}
