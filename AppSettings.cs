using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RaminelsLibrary
{
    public class AppSettings
    {
        public static string AppName => Assembly.GetEntryAssembly().GetName().Name;

        public static string AppDirectory => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public static string AppVersion => FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion;

        public static string? ConfigurationPath { get; set; }

        public static IConfiguration Configuration
        {
            get
            {
                string appSettingsPath = Path.Combine(AppDirectory, ConfigurationPath ?? "appsettings.json");

                IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(appSettingsPath)
                .Build();

                return config;
            }
        }

        public static string GetConnectionString(IConfiguration? configuration = null, string name = "DefaultConnection")
        {
            if (configuration is null)
                return Configuration.GetConnectionString(name) ?? Configuration.GetConnectionString("Default");

            return configuration.GetConnectionString(name) ?? configuration.GetConnectionString("Default");
        }

        public static T GetConfigurationObject<T>(IConfigurationSection section) where T : new()
        {
            T result = new();
            section.Bind(result);
            return result;
        }
    }
}
