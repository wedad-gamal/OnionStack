namespace Application.DTOs.Identity
{
    public class IdentityResultDto
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }

}
