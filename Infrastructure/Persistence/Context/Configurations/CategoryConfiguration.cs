namespace Infrastructure.Persistence.Context.Configurations;
internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(builder => builder.Name)
               .IsRequired()
               .HasMaxLength(100);
    }
}
