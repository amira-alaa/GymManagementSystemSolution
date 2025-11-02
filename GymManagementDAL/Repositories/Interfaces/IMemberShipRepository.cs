using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMemberShipRepository : IGenericRepository<MemberShip>
    {
        IEnumerable<MemberShip> GetMemberShips();

        MemberShip? GetByMemberIdAndPlanId(int memberId , int planId);
        //bool CreateMemberShip(MemberShip memberShip);
        //bool DeleteMemberShip(MemberShip memberShip);
    }
}
