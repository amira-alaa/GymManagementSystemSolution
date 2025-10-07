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
    internal class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly GymDbContext _dbContext;

        public HealthRecordRepository(GymDbContext dbContext)
        { 
            _dbContext = dbContext;

        }
        public int Add(HealthRecord HR)
        {
            _dbContext.HealthRecords.Add(HR);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var HR = _dbContext.HealthRecords.Find(id);
            if (HR is null) return 0;
            _dbContext.HealthRecords.Remove(HR);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<HealthRecord> GetAll() => _dbContext.HealthRecords.ToList();
        
        public HealthRecord? GetById(int id) => _dbContext.HealthRecords.Find(id);
   
        public int Update(HealthRecord HR)
        {
            _dbContext.HealthRecords.Update(HR);
            return _dbContext.SaveChanges();
        }
    }
}
