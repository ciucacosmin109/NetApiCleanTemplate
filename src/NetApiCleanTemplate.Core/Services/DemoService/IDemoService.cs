using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.Core.Services.DemoService.Input;
using NetApiCleanTemplate.Core.Services.DemoService.Output;

namespace NetApiCleanTemplate.Core.Services.DemoService;
public interface IDemoService
{
    Task<DemoDto> Get(int id);
    Task<ICollection<DemoDto>> GetAll();

    Task<int> Create(CreateDemoDto dto);
    Task Update(UpdateDemoDto dto);
    Task Delete(int id);

    Task ChangeParent(ChangeParentForDemoDto dto);
}
