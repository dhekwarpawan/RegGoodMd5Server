using RegGoodMd5Server.Controllers;

namespace RegGoodMd5Server
{
    public class Common_services
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DashboardController> _logger;

        public Common_services(IConfiguration configuration, ILogger<DashboardController> logger)
        {
            _configuration = configuration;
            _logger = logger;

        }


    }
}
