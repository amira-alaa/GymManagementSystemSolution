using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.MemberSessionViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberSessionService
    {
        IEnumerable<MemberSessionViewModel> GetNotCompletedMemberSessions();

        IEnumerable<MembersForOnGoingSessionsViewModel> GetMembersForOnGoingsSessions(int sessionId);
        bool IsAttended(int sessionId, int memberId);

        IEnumerable<MembersForUpComingSessionsViewModel> GetMembersForUpComingSessions(int sessionId);

        bool DeleteMemberSession(int sessionId, int memberId);

        bool CreateMemberSession(CreateMemberSessionViewModel CreatedMS);
        IEnumerable<MembersForDropListBookingViewModel> GetMembers();

    }
}
