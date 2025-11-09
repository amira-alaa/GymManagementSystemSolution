using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.MemberViewModel;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync();
        Task<bool> CreateMemberAsync(CreateMemberViewModel createMember);
        Task<MemberViewModel?> GetMemberByIdAsync(int id);

        Task<HealthRecordViewModel?> GetHealthRecordAsync(int id);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id);

        Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel memberToUpdate);
        Task<bool> RemoveMemberAsync(int id);

    }
}
