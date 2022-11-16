using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.Core.Extensions;

public static class JsonExtensions
{
    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public static T? FromJson<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json, _jsonOptions);
    }
    public static string ToJson<T>(this T obj)
    {
        return JsonSerializer.Serialize<T>(obj, _jsonOptions);
    }
}
