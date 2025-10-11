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

        IGenericRepository<T> GetRepository<T>() where T : BaseEntity, new();
        int SaveChanges();
    }
}
