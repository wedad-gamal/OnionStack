using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Context.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Ignore(u => u.EmailAddress); // don’t create separate VO column

            // Map base.Email normally (Identity default)
            builder.Property(u => u.Email)
                   .HasColumnName("Email")
                   .IsRequired();
        }
    }
}
