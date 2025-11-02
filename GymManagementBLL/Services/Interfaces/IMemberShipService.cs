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
        bool CreateMemberShip(CreateMemberShipViewModel memberShipViewModel);
        MemberShip? GetMemberShipByIDs(int MemberId , int PlanId);

        IEnumerable<SelectMemberToDropListViewModel> GetMembers();
        IEnumerable<SelectPlanToDropListViewModel> GetPlans();
        bool DeleteMemberShip(MemberShip memberShip);
    }
}
