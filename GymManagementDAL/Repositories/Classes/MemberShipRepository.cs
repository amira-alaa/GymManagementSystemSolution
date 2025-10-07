using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.Classes
{
    internal class MemberShipRepository : IMemberShipRepository
    {
        private readonly GymDbContext _dbContext;

        public MemberShipRepository(GymDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }
        public int Add(MemberShip membership)
        {
            _dbContext.MemberShips.Add(membership);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var membership = _dbContext.MemberShips.Find(id);
            if (membership is null) return 0;
            _dbContext.MemberShips.Remove(membership);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<MemberShip> GetAll() => _dbContext.MemberShips.ToList();

        public MemberShip? GetById(int id) => _dbContext.MemberShips.Find(id);

        public int Update(MemberShip membership)
        {
            _dbContext.MemberShips.Update(membership);
            return _dbContext.SaveChanges();
        }
    }
}
