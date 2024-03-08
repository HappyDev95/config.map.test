namespace config.map.test.Service
{
    public class Service : IService
    {
        private readonly IConfiguration _configuration;

        public Service(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ReadFromConfigMapExample()
        {
            try
            {
                var content = File.ReadAllText(_configuration.config_map_mount_path);
                return content;
            }
            catch (Exception ex)
            {
                return "FAILURE";
            }
        }
    }
}
