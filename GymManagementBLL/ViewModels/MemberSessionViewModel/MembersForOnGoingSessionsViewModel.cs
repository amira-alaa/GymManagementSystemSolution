using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberSessionViewModel
{
    public class MembersForOnGoingSessionsViewModel
    {
        public int SessionId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } = null!;
        public bool IsAttended { get; set; }

    }
}
