using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Entities
{
    [Owned]
    public class Address
    {
        public int BulildingNumber { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
