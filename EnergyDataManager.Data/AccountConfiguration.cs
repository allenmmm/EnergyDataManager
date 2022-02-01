using EnergyDataManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnergyDataManager.Data
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> modelBuilder)
        {
            modelBuilder.Property(m => m.Id).ValueGeneratedNever();
            var navigation = modelBuilder
               .Metadata.FindNavigation(nameof(Account.Readings));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Property(m => m.FirstName)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder.Property(m => m.LastName)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
