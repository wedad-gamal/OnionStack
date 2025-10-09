﻿namespace Application.Abstraction.Interfaces.Common
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    }
}
