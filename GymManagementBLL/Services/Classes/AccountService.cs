using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModel;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManagementBLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ApplicationUser?> ValidateAsync(AccountViewModel viewModel)
        {
            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user is null) return null;
            
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, viewModel.Password);
            return isPasswordValid ? user : null;
        }
    }
}
