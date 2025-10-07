using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    internal interface IMemberShipRepository
    {
        // Get All
        IEnumerable<MemberShip> GetAll();
        // Get By Id
        MemberShip? GetById(int id);

        // Add
        int Add(MemberShip membership);
        // Update
        int Update(MemberShip membership);
        // Delete
        int Delete(int id);
    }
}
