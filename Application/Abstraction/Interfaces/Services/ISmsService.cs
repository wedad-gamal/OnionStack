namespace Abstraction.Abstraction.Interfaces.Services
{
    public interface ISmsService
    {
        Task SendMessageAsync(MessageDto messageDto);
    }
}
