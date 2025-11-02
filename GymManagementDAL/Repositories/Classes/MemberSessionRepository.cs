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

        public int GetNumOfBookedSlots(int sessionId)
        {
            return _dbContext.MemberSessions.Count(s => s.SessionId == sessionId);
        }
        public IEnumerable<MemberSession> GetNotCompletedMemberSessions()
        {
            return _dbContext.MemberSessions
                             .Include(ms => ms.Session)
                             .ThenInclude(s => s.SessionTrainer)
                             .Include(ms => ms.Session)
                             .ThenInclude(s => s.SessionCategory)
                             .Where(ms => ms.Session.EndDate > DateTime.Now)
                             .GroupBy(ms => new { ms.SessionId, ms.Session.TrainerId })
                             .Select(g => g.First()) 
                             .ToList();
        }

        public IEnumerable<MemberSession> GetMembersForOnGoing(int sessionId)
        {
            return _dbContext.MemberSessions
                             .Include(ms => ms.Member)
                             .Include(ms => ms.Session)
                             .Where(ms => ms.SessionId == sessionId 
                                    && ms.Session.EndDate > DateTime.Now 
                                    && ms.Session.StartDate <= DateTime.Now)
                             .ToList();
        }

        public IEnumerable<MemberSession> GetMembersForUpComing(int sessionId)
        {
            return _dbContext.MemberSessions
                             .Include(ms => ms.Member)
                             .Include(ms => ms.Session)
                             .Where(ms => ms.SessionId == sessionId
                             && ms.Session.StartDate > DateTime.Now)
                             .ToList();
        }

        public IEnumerable<Member> GetMembers()
        {
            return _dbContext.Members
                             .ToList();
        }
    }
}
