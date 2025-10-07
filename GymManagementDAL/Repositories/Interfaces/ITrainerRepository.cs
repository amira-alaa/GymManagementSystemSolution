using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    internal interface ITrainerRepository
    {
        // Get All
        IEnumerable<Trainer> GetAll();
        // Get By Id
        Trainer? GetById(int id);

        // Add
        int Add(Trainer trainer);
        // Update
        int Update(Trainer trainer);
        // Delete
        int Delete(int id);
    }
}
