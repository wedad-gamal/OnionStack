namespace Infrastructure.Mapping;
internal class CategoryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryDto>()
            .Map(c => c.Id, c => c.Id)
            .Map(c => c.Name, c => c.Name)
            .Map(c => c.IsDeleted, c => c.IsDeleted)
            .Map(c => c.CreatedOn, c => c.CreatedOn)
            .Map(dest => dest.ModifiedOn, src => src.ModifiedOn)
            .Map(dest => dest.ModifiedOnFormated, src => src.ModifiedOn.ToString());

    }
}
