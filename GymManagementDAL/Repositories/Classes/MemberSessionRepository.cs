using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.Classes
{
    internal class MemberSessionRepository : IMemberSessionRepository
    {

        private readonly GymDbContext _dbContext;

        public MemberSessionRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(MemberSession memberSession)
        {
            _dbContext.MemberSessions.Add(memberSession);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var membersession = _dbContext.MemberSessions.Find(id);
            if (membersession is null) return 0;
            _dbContext.MemberSessions.Remove(membersession);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<MemberSession> GetAll() => _dbContext.MemberSessions.ToList();

        public MemberSession? GetById(int id) => _dbContext.MemberSessions.Find(id);

        public int Update(MemberSession memberSession)
        {
            _dbContext.MemberSessions.Update(memberSession);
            return _dbContext.SaveChanges();
        }
    }
}
