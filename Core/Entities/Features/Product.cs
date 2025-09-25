using Core.Common;

namespace Core.Entities.Features;
public class Product : BaseEntity<Guid>
{
    public string Name { get; set; }
    public int Stock { get; set; }
}
