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
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("SessionCapacityCheck", "Capacity Between 1 and 25");
                tb.HasCheckConstraint("SessionEndDateCheck", "EndDate > StartDate");
            });

            builder.HasOne(s => s.SessionCategory)
                   .WithMany(c => c.Sessions)
                   .HasForeignKey(s => s.CategoryId);

            builder.HasOne(T => T.SessionTrainer)
                    .WithMany(S => S.Sessions)
                    .HasForeignKey(S => S.TrainerId);
        }
    }
}
