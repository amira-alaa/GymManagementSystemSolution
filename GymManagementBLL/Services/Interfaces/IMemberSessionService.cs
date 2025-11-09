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
        Task<IEnumerable<MemberSessionViewModel>> GetNotCompletedMemberSessionsAsync();

        Task<IEnumerable<MembersForOnGoingSessionsViewModel>> GetMembersForOnGoingsSessionsAsync(int sessionId);
        Task<bool> IsAttendedAsync(int sessionId, int memberId);

        Task<IEnumerable<MembersForUpComingSessionsViewModel>> GetMembersForUpComingSessionsAsync(int sessionId);

        Task<bool> DeleteMemberSessionAsync(int sessionId, int memberId);

        Task<bool> CreateMemberSessionAsync(CreateMemberSessionViewModel CreatedMS);
        Task<IEnumerable<MembersForDropListBookingViewModel>> GetMembersAsync();

    }
}
