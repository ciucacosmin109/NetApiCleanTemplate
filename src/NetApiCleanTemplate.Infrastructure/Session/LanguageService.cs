using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.SharedKernel.Interfaces.Session;

namespace NetApiCleanTemplate.Infrastructure.Session;

public class LanguageService : ILanguageService
{
    public const string LanguageCodeHeader = "languageCode";

    private const string defaultLanguageCode = "en"; 
    private string currentLanguageCode = defaultLanguageCode;  

    public string GetCode()
    {
        return currentLanguageCode;
    }

    public void SetCode(string langCode)
    {
        currentLanguageCode = langCode;
    }
}
