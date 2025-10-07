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
    internal class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(p => p.Description)
                   .HasColumnType("varchar")
                   .HasMaxLength(200);

            builder.Property(p => p.Price)
                    .HasPrecision(10, 2);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("PlanDurationDaysCheck", "DurationDays Between 1 and 365");
            });
        }
    }
}
