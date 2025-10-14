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
        IEnumerable<Session> GetSessionsWithTrainersAndCategories();
        Session? GetSessionByIdWithTrainersAndCategories(int id);
        int GetNumOfBookedSlots(int sessionId);
    }
}
