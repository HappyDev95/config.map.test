using System;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace config.map.test
{
    public class Program
    {
        public static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        public static void Shutdown(int? exitCode = null)
        {
            Environment.ExitCode = exitCode ?? 0;
            CancellationTokenSource.Cancel();
        }

        public static void Main(string[] args)
        {
            var serviceName = Assembly.GetEntryAssembly().EntryPoint.DeclaringType.Namespace;
            var serviceVersion = $"{Assembly.GetEntryAssembly().GetName().Version} [{Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "UNKNOWN"}]";

            var context = new Dictionary<string, string>();
            context.Add("ClientName", "client-name");
            context.Add("CorrelationID", Guid.NewGuid().ToString());

            IConfiguration configuration = new Configuration();

            try
            {
                WebHost.CreateDefaultBuilder()
                    .ConfigureAppConfiguration((_, config) => config.Sources.Clear())
                    .ConfigureLogging(config => { config.ClearProviders(); })
                    .SuppressStatusMessages(true)
                    .UseUrls(configuration.http_prefix)
                    .ConfigureServices(s =>
                    {
                        s.AddSingleton<IConfiguration>(configuration);
                    })
                    .UseStartup<Startup>()
                    .Build()
                    .RunAsync(CancellationTokenSource.Token)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
