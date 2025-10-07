using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    internal interface IHealthRecordRepository
    {
        // Get All
        IEnumerable<HealthRecord> GetAll();
        // Get By Id
        HealthRecord? GetById(int id);

        // Add
        int Add(HealthRecord HR);
        // Update
        int Update(HealthRecord HR);
        // Delete
        int Delete(int id);
    }
}
