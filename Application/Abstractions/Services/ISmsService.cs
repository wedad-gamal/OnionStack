namespace Application.Abstractions.Services
{
    public interface ISmsService
    {
        Task SendMessageAsync(MessageDto messageDto);
    }
}
