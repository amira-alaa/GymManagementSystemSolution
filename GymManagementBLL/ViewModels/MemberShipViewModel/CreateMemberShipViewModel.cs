using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementBLL.ViewModels.MemberShipViewModel
{
    public class CreateMemberShipViewModel
    {
        [Required(ErrorMessage = "Member is Required")]
        public int MemberId { get; set; }
        [Required(ErrorMessage = "Plan is Required")]
        public int PlanId { get; set; }

        //public DateTime? EndDate { get; set; }

    }
}
