﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace NetApiCleanTemplate.SharedKernel.Configuration;

public static class AppConfigurations
{
    private static readonly ConcurrentDictionary<string, IConfigurationRoot> _configurationCache;

    static AppConfigurations()
    {
        _configurationCache = new ConcurrentDictionary<string, IConfigurationRoot>();
    }

    public static IConfigurationRoot Get(string? path = null, string? environmentName = null, bool addUserSecrets = false)
    {
        path ??= CalculateContentRootFolder();

        var cacheKey = path + "#" + environmentName + "#" + addUserSecrets;
        return _configurationCache.GetOrAdd(
            cacheKey,
            _ => BuildConfiguration(path, environmentName, addUserSecrets)
        );
    }

    private static IConfigurationRoot BuildConfiguration(string path, string? environmentName = null, bool addUserSecrets = false)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        if (!String.IsNullOrWhiteSpace(environmentName))
        {
            builder = builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true);
        }

        builder = builder.AddEnvironmentVariables();

        if (addUserSecrets)
        {
            builder.AddUserSecrets(typeof(AppConfigurations).Assembly);
        }

        return builder.Build();
    }

    public static string CalculateContentRootFolder()
    {
        var assemblyDirectoryPath = Path.GetDirectoryName(typeof(AppConfigurations).Assembly.Location);
        if (assemblyDirectoryPath == null)
        {
            throw new Exception("Could not find the location of the current assembly!");
        }

        var directoryInfo = new DirectoryInfo(assemblyDirectoryPath);
        while (!DirectoryContains(directoryInfo.FullName, "NetApiCleanTemplate.sln"))
        {
            if (directoryInfo.Parent == null)
            {
                throw new Exception("Could not find the content root folder!");
            }

            directoryInfo = directoryInfo.Parent;
        }

        var webApiFolder = Path.Combine(directoryInfo.FullName, "src", "NetApiCleanTemplate.WebApi");
        if (Directory.Exists(webApiFolder))
        {
            return webApiFolder;
        }

        throw new Exception("Could not find the root folder of the web api project!");
    }

    private static bool DirectoryContains(string directory, string fileName)
    {
        return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
    }
}