using Core.Entities.Common;

namespace Core.Entities;
public class Product : BaseEntity<Guid>
{
    public string Name { get; set; }
    public int Stock { get; set; }
}
