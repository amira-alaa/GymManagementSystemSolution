using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberShipViewModel
{
    public class MemberShipViewModel
    {
        //public int Id { get; set; }
        public int MemberId { get; set; }
        public int PlanId { get; set; }
        public string MemberName { get; set; } = null!;
        public string PlanName { get; set; } = null!;
        public DateTime MemberShipStartDate { get; set; }
        public DateTime MemberShipEndDate { get; set; } 

        // calculation props

        public string DisplayStartDate => MemberShipStartDate.ToString("MMM dd, yyyy");
        public string DisplayEndDate => MemberShipEndDate.ToString("MMM dd, yyyy");
    }
}
