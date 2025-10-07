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
    internal class MemberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Property(p => p.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValueSql("GETDATE()");

            builder.HasKey(ms => new { ms.MemberId, ms.SessionId });
            builder.Ignore(ms => ms.Id);

        }
    }
}
