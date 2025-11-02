using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberSessionViewModel
{
    public class MembersForUpComingSessionsViewModel
    {
        public int MemberID { get; set; }
        public int SessionId { get; set; }
        public string MemberName { get; set; } = null!;
        public DateTime BookingDate { get; set; }

    }
}
