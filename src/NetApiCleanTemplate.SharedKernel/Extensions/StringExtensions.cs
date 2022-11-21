using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Extensions;
public static class StringExtensions
{
    /// <summary>
    /// Align a multiline string from the indentation of its first line
    /// </summary>
    /// <remarks>The </remarks>
    /// <param name="source">The string to align</param>
    /// <returns></returns>
    public static string RemoveTheFirstIndentationLevel(this string source)
    {
        if (String.IsNullOrEmpty(source))
        {
            return source;
        }

        if (!source.StartsWith(Environment.NewLine))
        {
            throw new FormatException("String must start with a NewLine character.");
        }

        int indentationSize = source
                                .Skip(Environment.NewLine.Length)
                                .TakeWhile(Char.IsWhiteSpace)
                                .Count();

        string indentationStr = new string(' ', indentationSize);
        return source.TrimStart().Replace($"\n{indentationStr}", "\n");
    }
}
