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
    internal class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members")
                   .HasKey(p => p.Id);

            builder.HasOne<Member>()
                   .WithOne(M => M.HealthRecord)
                   .HasForeignKey<HealthRecord>(HR => HR.Id);
            builder.Ignore(p => p.CreatedAt);
        }
    }
}
