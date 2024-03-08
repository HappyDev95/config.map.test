using System.Text;

namespace config.map.test
{
    public interface IConfiguration
    {
        string http_prefix { get; }
        int http_client_timeout { get; }

        int min_worker_threads { get; }
        int min_io_threads { get; }

        string config_map_mount_path { get; }
    }

    public class Configuration : IConfiguration
    {
        private const string HTTP_PREFIX = "http://*:9654";
        private const string HTTP_CLIENT_TIMEOUT = "60000";
        private const string MIN_WORKER_THREADS = "100";
        private const string MIN_IO_THREADS = "100";

        private const string CONIG_MAP_MOUNT_PATH = "/mnt/configmap/configuration.json";

        public string http_prefix { get; init; }
        public int http_client_timeout { get; init; }

        public int min_worker_threads { get; init; }
        public int min_io_threads { get; init; }

        public string config_map_mount_path { get; init; }

        public Configuration()
        {
            http_prefix = Environment.GetEnvironmentVariable(nameof(HTTP_PREFIX)) ?? HTTP_PREFIX;
            http_client_timeout = int.Parse(Environment.GetEnvironmentVariable(nameof(HTTP_CLIENT_TIMEOUT)) ?? HTTP_CLIENT_TIMEOUT);
            min_worker_threads = int.Parse(Environment.GetEnvironmentVariable(nameof(MIN_WORKER_THREADS)) ?? MIN_WORKER_THREADS);
            min_io_threads = int.Parse(Environment.GetEnvironmentVariable(nameof(MIN_IO_THREADS)) ?? MIN_IO_THREADS);

            config_map_mount_path = Environment.GetEnvironmentVariable(nameof(CONIG_MAP_MOUNT_PATH)) ?? CONIG_MAP_MOUNT_PATH;
        }

        public override string ToString()
        {
            var configurationSummary = new StringBuilder();
            configurationSummary.AppendLine("CONFIGURATION :");
            configurationSummary.AppendLine(string.Empty.PadRight(80, '*'));
            configurationSummary.AppendLine($"{nameof(http_prefix)}:  {http_prefix}");
            configurationSummary.AppendLine();
            configurationSummary.AppendLine($"{nameof(http_client_timeout)}: {http_client_timeout}");
            configurationSummary.AppendLine();
            configurationSummary.AppendLine($"{nameof(min_worker_threads)}: {min_worker_threads}");
            configurationSummary.AppendLine($"{nameof(min_io_threads)}: {min_io_threads}");
            configurationSummary.AppendLine();
            configurationSummary.AppendLine($"{nameof(config_map_mount_path)}: {config_map_mount_path}");
            configurationSummary.AppendLine();
            configurationSummary.AppendLine(string.Empty.PadRight(80, '*'));
            return configurationSummary.ToString();
        }
    }
}
