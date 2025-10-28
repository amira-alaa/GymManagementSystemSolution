using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        // joinDate == createdAt in baseEntity
        public string Photo { get; set; } = null!;

        #region Relationships
        #region HealthRecord - Member (1 - 1)
        public HealthRecord HealthRecord { get; set; } = null!;
        #endregion

        #region Member - MemberShip ( 1 - M )
        public ICollection<MemberShip> MemberShips { get; set; } = null!;

        #endregion

        #region Member - MemberSession ( 1 - M )
        public ICollection<MemberSession> MemberSessions { get; set; } = null!;
        #endregion
        #endregion
    }
}
