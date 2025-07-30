namespace Core.Results
{
    public class IdentityResultDto
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }

}
