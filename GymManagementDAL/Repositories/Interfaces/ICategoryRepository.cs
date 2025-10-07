using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    internal interface ICategoryRepository
    {
        // Get All
        IEnumerable<Category> GetAll();
        // Get By Id
        Category? GetById(int id);

        // Add
        int Add(Category category);
        // Update
        int Update(Category category);
        // Delete
        int Delete(int id);
    }
}
