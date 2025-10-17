using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.AnalyticesViewModel
{
    public class AnalyticesViewModel
    {
        public int TotalMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int Trainers { get; set; }
        public int UpComingSessions { get; set; }
        public int OnGoingSessions { get; set; }
        public int CompletedSessions { get; set; }
    }
}
