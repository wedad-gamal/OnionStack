using Application.DTOs.Common;

namespace Application.Abstraction.Interfaces.Common
{
    public interface ISmsService
    {
        Task SendMessageAsync(MessageDto messageDto);
    }
}
