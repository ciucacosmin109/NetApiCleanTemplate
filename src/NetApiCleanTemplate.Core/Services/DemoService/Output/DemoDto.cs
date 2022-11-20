using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.Core.Entities.DemoEntity;

namespace NetApiCleanTemplate.Core.Services.DemoService.Output;
public class DemoDto
{
    public int Id { get; set; }
    public string DemoString { get; set; } = String.Empty;

    public ICollection<DemoDto> DemoChildren { get; set; } = new List<DemoDto>();

    public static DemoDto ToDto(DemoEntity entity)
    {
        var dto = new DemoDto {
            DemoString = entity.DemoString,
            Id = entity.Id,
            DemoChildren = ToDto(entity.DemoChildren)
        };
        return dto;
    }
    public static ICollection<DemoDto> ToDto(ICollection<DemoEntity> entityList)
    {
        var list = new List<DemoDto>();
        foreach (var child in entityList)
        {
            list.Add(ToDto(child));
        }
        return list;
    }
}
