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
        Task<IEnumerable<MemberSession>> GetNotCompletedMemberSessionsAsync();
        Task<int> GetNumOfBookedSlotsAsync(int sessionId);

        Task<IEnumerable<MemberSession>> GetMembersForOnGoingAsync(int sessionId);

        Task<IEnumerable<MemberSession>> GetMembersForUpComingAsync(int sessionId);

        //IEnumerable<Member> GetMembers();

    }
}
