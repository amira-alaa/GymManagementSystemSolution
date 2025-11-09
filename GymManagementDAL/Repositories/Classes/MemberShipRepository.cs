using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class MemberShipRepository : GenericRepository<MemberShip> , IMemberShipRepository
    {
        private readonly GymDbContext _dbContext;

        public MemberShipRepository(GymDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<MemberShip?> GetByMemberIdAndPlanIdAsync(int memberId, int planId)
        {
            return await _dbContext.MemberShips
                             .Where(ms => ms.MemberId == memberId && ms.PlanId == planId).FirstOrDefaultAsync();
        }

        public IEnumerable<MemberShip> GetMemberShips()
        {
            return _dbContext.MemberShips.Include(ms => ms.Member)
                                        .Include(ms => ms.Plan)
                                        .AsNoTracking()
                                        .AsEnumerable()
                                        .Where(ms => ms.Status == "Active");
                                       
        }

      
    }
}
