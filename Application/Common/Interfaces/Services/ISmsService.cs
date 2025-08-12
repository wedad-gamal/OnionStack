namespace Application.Common.Interfaces.Services
{
    public interface ISmsService
    {
        Task SendMessageAsync(MessageDto messageDto);
    }
}
