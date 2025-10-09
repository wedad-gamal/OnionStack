using Core.Entities.Common;

namespace Core.Entities;
public class Category : BaseEntity<int>
{
    public string Name { get; set; } = default!;
    public bool IsDeleted { get; set; }
}
