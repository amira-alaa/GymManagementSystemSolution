using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    internal class MemberShipConfiguration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.Property(p => p.CreatedAt)
                    .HasColumnName("StartDate")
                   .HasDefaultValueSql("GETDATE()");
            builder.HasKey(p => new { p.MemberId, p.PlanId });
            builder.Ignore(p => p.Id);
        }
    }
}
