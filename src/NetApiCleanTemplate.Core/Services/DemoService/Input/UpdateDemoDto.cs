using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.Core.Services.DemoService.Input;
public class UpdateDemoDto
{
    public int Id { get; set; }
    public string? DemoString { get; set; }
}
