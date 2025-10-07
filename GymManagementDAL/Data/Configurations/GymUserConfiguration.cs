using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GymManagementDAL.Data.Configurations
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(p => p.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(p => p.Email)
                  .HasColumnType("varchar")
                  .HasMaxLength(100);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("GymUserEmailCheck", "Email Like '_%@_%._%'");
                tb.HasCheckConstraint("GymUserPhoneCheck", "Phone Like '01%' and Phone Not Like '%[^0-9]%'");
            });

            builder.HasIndex(p => p.Email).IsUnique();

            builder.HasIndex(p => p.Phone).IsUnique();


            builder.Property(p => p.Phone)
                 .HasColumnType("varchar")
                 .HasMaxLength(11);

            builder.OwnsOne(NP => NP.Address, Address =>
            {
                Address.Property(P => P.Street)
                        .HasColumnName("Street")
                        .HasColumnType("varchar")
                        .HasMaxLength(30);
                Address.Property(P => P.City)
                        .HasColumnName("City")
                        .HasColumnType("varchar")
                        .HasMaxLength(30);
                Address.Property(P => P.BulildingNumber)
                        .HasColumnName("BulildingNumber");
            });




        }
    }
}
