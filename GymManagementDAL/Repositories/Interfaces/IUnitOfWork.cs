using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public ISessionRepository SessionRepository { get; }

        public IMemberSessionRepository MemberSessionRepository { get; }
        public IMemberShipRepository MemberShipRepository { get; }

        IGenericRepository<T> GetRepository<T>() where T : BaseEntity, new();
        Task<int> SaveChangesAsync();
    }
}
