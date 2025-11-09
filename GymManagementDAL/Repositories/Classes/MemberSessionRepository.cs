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
    public class MemberSessionRepository :GenericRepository<MemberSession> , IMemberSessionRepository
    {
        private readonly GymDbContext _dbContext;

        public MemberSessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetNumOfBookedSlotsAsync(int sessionId)
        {
            return await _dbContext.MemberSessions.CountAsync(s => s.SessionId == sessionId);
        }
        public async Task<IEnumerable<MemberSession>> GetNotCompletedMemberSessionsAsync()
        {
            return await _dbContext.MemberSessions
                             .Include(ms => ms.Session)
                             .ThenInclude(s => s.SessionTrainer)
                             .Include(ms => ms.Session)
                             .ThenInclude(s => s.SessionCategory)
                             .Where(ms => ms.Session.EndDate > DateTime.Now)
                             .GroupBy(ms => new { ms.SessionId, ms.Session.TrainerId })
                             .Select(g => g.First()) 
                             .ToListAsync();
        }

        public async Task<IEnumerable<MemberSession>> GetMembersForOnGoingAsync(int sessionId)
        {
            return await _dbContext.MemberSessions
                             .Include(ms => ms.Member)
                             .Include(ms => ms.Session)
                             .Where(ms => ms.SessionId == sessionId 
                                    && ms.Session.EndDate > DateTime.Now 
                                    && ms.Session.StartDate <= DateTime.Now)
                             .ToListAsync();
        }

        public async Task<IEnumerable<MemberSession>> GetMembersForUpComingAsync(int sessionId)
        {
            return await _dbContext.MemberSessions
                             .Include(ms => ms.Member)
                             .Include(ms => ms.Session)
                             .Where(ms => ms.SessionId == sessionId
                             && ms.Session.StartDate > DateTime.Now)
                             .ToListAsync();
        }

        //public IEnumerable<Member> GetMembers()
        //{
        //    return _dbContext.Members
        //                     .ToList();
        //}
    }
}
