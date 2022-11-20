using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.Core.Services.DemoService.Input;
public class CreateDemoDto
{
    public string? DemoString { get; set; }
    public int? DemoParentId { get; set; }
}
