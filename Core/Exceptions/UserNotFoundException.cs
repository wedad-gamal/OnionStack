namespace Core.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string userId) : base($"User Id {userId} Not Found.")
        {
        }
    }
}
