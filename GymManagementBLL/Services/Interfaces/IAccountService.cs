using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.AccountViewModel;
using GymManagementDAL.Entities;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IAccountService
    {
        ApplicationUser? Validate(AccountViewModel viewModel);
    }
}
