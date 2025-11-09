using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.MemberShipViewModel;
using GymManagementDAL.Entities;
//using GymManagementDAL.Entities;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberShipService 
    {
        IEnumerable<MemberShipViewModel> GetAllMemberShips();
        Task<bool> CreateMemberShipAsync(CreateMemberShipViewModel memberShipViewModel);
        Task<MemberShip?> GetMemberShipByIDsAsync(int MemberId , int PlanId);

        Task<IEnumerable<SelectMemberToDropListViewModel>> GetMembersAsync();
        Task<IEnumerable<SelectPlanToDropListViewModel>> GetPlansAsync();
        Task<bool> DeleteMemberShipAsync(MemberShip memberShip);
    }
}
