using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface ISessionService
    {
        IEnumerable<SessionViewModel> Index();
        SessionViewModel? GetSessionById(int id);

        bool CreateSession(CreateSessionViewModel session);

        UpdateSessionViewModel? GetSessionForUpdate(int id);
        bool UpdateSession(UpdateSessionViewModel UpdateSession , int Id);
        bool RemoveSession(int id);
    }
}
