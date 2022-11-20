using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.SharedKernel.Entities;

namespace NetApiCleanTemplate.Core.Entities.DemoEntity;

public class DemoEntity : BaseEntity
{
    public string DemoString { get; set; } = String.Empty;

    public int? DemoParentId { get; set; }
    public DemoEntity? DemoParent { get; set; }

    public ICollection<DemoEntity> DemoChildren { get; set; } = new List<DemoEntity>();
}

