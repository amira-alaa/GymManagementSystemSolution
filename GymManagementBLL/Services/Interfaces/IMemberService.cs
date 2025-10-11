using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.MemberViewModel;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface IMemberService
    {
        IEnumerable<MemberViewModel> Index();
        bool Create(CreateMemberViewModel createMember);
        MemberViewModel? GetMember(int id);

        HealthRecordViewModel? GetHealthRecord(int id);
        MemberToUpdateViewModel? GetMemberToUpdate(int id);

        bool UpdateMemberDetails(int id, MemberToUpdateViewModel memberToUpdate);
        bool RemoveMember(int id);

    }
}
