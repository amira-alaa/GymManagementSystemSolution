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
    internal class TrainerConfiguration : GymUserConfiguration<Trainer> ,IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(p => p.CreatedAt)
                   .HasColumnName("HireDate")
                   .HasDefaultValueSql("GETDATE()");

            base.Configure(builder);
        }
    }
}
