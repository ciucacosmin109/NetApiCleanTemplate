using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaCleanTemplate.Core.Entities.DemoEntity;

public class DemoEntity : BaseEntity
{
    public string? DemoString { get; set; }

    public int? DemoParentId { get; set; }
    public DemoEntity? DemoParent { get; set; }

    public ICollection<DemoEntity>? DemoChildren { get; set; }
}
