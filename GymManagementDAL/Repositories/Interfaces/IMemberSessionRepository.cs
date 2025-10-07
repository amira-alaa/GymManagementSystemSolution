using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    internal interface IMemberSessionRepository
    {
        // Get All
        IEnumerable<MemberSession> GetAll();
        // Get By Id
        MemberSession? GetById(int id);
        // Add
        int Add(MemberSession memberSession);
        // Update
        int Update(MemberSession memberSession);
        // Delete
        int Delete(int id);
    }
}
