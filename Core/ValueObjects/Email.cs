namespace Core.ValueObjects
{
    public class Email
    {
        public string Value { get; private set; }

        private Email() { } // EF Core

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty.", nameof(value));

            if (!IsValidEmail(value))
                throw new ArgumentException("Invalid email format.", nameof(value));

            Value = value;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString() => Value;

        public static implicit operator string(Email email) => email.Value;
        public static implicit operator Email(string email) => new Email(email);
    }
}
