using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync();
        Task<SessionViewModel?> GetSessionByIdAsync(int id);

        Task<bool> CreateSessionAsync(CreateSessionViewModel session);

        Task<UpdateSessionViewModel?> GetSessionForUpdateAsync(int id);
        Task<bool> UpdateSessionAsync(UpdateSessionViewModel UpdateSession , int Id);
        Task<bool> RemoveSessionAsync(int id);

        Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainerForDropDownAsync();
        Task<IEnumerable<CategorySelectViewModel>> GetAllCategoryForDropDownAsync();

    }
}
