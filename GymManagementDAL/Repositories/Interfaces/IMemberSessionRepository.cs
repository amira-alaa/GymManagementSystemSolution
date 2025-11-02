using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMemberSessionRepository : IGenericRepository<MemberSession>
    {
        IEnumerable<MemberSession> GetNotCompletedMemberSessions();
        public int GetNumOfBookedSlots(int sessionId);

        IEnumerable<MemberSession> GetMembersForOnGoing(int sessionId);

        IEnumerable<MemberSession> GetMembersForUpComing(int sessionId);

        IEnumerable<Member> GetMembers();

    }
}
