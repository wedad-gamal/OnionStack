using Core.Entities.Features;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Context.Configurations
{
    class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            //builder.HasKey(e => e.UserId);

            //builder.Property(e => e.Salary).HasColumnType("decimal(18,2)");
        }
    }
}
