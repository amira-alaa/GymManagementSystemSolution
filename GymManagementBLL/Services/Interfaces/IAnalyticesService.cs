using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.AnalyticesViewModel;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IAnalyticesService
    {
        AnalyticesViewModel GetAnalyticesData();
    }
}
