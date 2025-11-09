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

        public async Task<int> GetNumOfBookedSlotsAsync(int sessionId)
        {
            return await _dbContext.MemberSessions.CountAsync(s => s.SessionId == sessionId);
        }

        public async Task<Session?> GetSessionByIdWithTrainersAndCategoriesAsync(int id)
        {
            return await _dbContext.Sessions.Include(s => s.SessionTrainer)
                                      .Include(s => s.SessionCategory)
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(s => s.Id == id);
        }
       

        public async Task<IEnumerable<Session>> GetSessionsWithTrainersAndCategoriesAsync()
        {
            return await _dbContext.Sessions.Include(s => s.SessionTrainer)
                                      .Include(s => s.SessionCategory)
                                      .Include(s => s.MemberSessions)
                                      .AsNoTracking().ToListAsync();
        }
    }
}
