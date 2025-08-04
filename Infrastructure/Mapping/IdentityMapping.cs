namespace Infrastructure.Mapping
{
    public class IdentityMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserDto, ApplicationUser>()
                  .Map(dest => dest.UserName, src => src.UserName)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.Age, src => src.Age)
                .TwoWays();

            config.NewConfig<CreateUserDto, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.UserName)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.Age, src => src.Age)
                .TwoWays();

        }
    }
}
