using Core.Common;

namespace Core.Entities.Features;
public class Category : BaseEntity<int>
{
    public string Name { get; set; } = default!;
    public bool IsDeleted { get; set; }
}
