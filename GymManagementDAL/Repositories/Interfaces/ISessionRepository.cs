using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetSessionsWithTrainersAndCategoriesAsync();
        Task<Session?> GetSessionByIdWithTrainersAndCategoriesAsync(int id);
        Task<int> GetNumOfBookedSlotsAsync(int sessionId);
    }
}
