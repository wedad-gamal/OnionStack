using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [NotMapped]
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }


    }
}
