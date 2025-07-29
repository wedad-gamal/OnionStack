namespace Core.Results
{
    public class IdentityResultDto
    {
        public bool Success { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }

}
