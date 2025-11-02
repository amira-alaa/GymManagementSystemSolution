using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberSessionViewModel
{
    public class CreateMemberSessionViewModel
    {
        [Required (ErrorMessage ="Member is Required")]
        public int MemberId { get; set; }
        public int SessionId { get; set; }
        //public string MemberName { get; set; } = null!;
    }
}
