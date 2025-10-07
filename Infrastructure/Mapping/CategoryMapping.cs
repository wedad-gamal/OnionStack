namespace Infrastructure.Mapping;
internal class CategoryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.IsDeleted, src => src.IsDeleted)
            .Map(dest => dest.CreatedOn, src => src.CreatedOn)
            .Map(dest => dest.ModifiedOn, srsrc => srsrc.ModifiedOn)
            .Map(dest => dest.ModifiedOnFormated, srsrc => srsrc.ModifiedOn.ToString());

        config.NewConfig<Category, CreateEditCategortDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);

    }
}
