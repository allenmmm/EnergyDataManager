using EnergyDataManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnergyDataManager.Data
{
    public class ReadingConfiguration : IEntityTypeConfiguration<Reading>
    {
        public void Configure(EntityTypeBuilder<Reading> modelBuilder)
        {
            modelBuilder.Property(m => m.Id).ValueGeneratedOnAdd();
            modelBuilder.OwnsOne(i => i.MeterReading,
                p => {
                    p.Property(mr => mr.Date)
                        .HasColumnType("smalldatetime")
                       .IsRequired();

                    p.Property(mr => mr.Value)
                        .IsRequired()
                        .HasMaxLength(5)
                        .IsFixedLength();
                });
            modelBuilder.Navigation(mr => mr.MeterReading).IsRequired();
            modelBuilder.Property(i => i.AccountId).IsRequired();
        }
    }
}
