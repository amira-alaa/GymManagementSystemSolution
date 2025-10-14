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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public int GetNumOfBookedSlots(int sessionId)
        {
            return _dbContext.MemberSessions.Count(s => s.SessionId == sessionId);
        }

        public Session? GetSessionByIdWithTrainersAndCategories(int id)
        {
            return _dbContext.Sessions.Include(s => s.SessionTrainer)
                                      .Include(s => s.SessionCategory)
                                      .AsNoTracking()
                                      .FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Session> GetSessionsWithTrainersAndCategories()
        {
            return _dbContext.Sessions.Include(s => s.SessionTrainer)
                                      .Include(s => s.SessionCategory)
                                      .AsNoTracking().ToList();
        }
    }
}
