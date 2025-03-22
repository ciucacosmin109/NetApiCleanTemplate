using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Interfaces.Session;

public interface ILanguageService
{
    void SetCode(string langCode);
    string GetCode();
}
