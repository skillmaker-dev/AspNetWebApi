using Microsoft.AspNetCore.Mvc;

namespace PSWebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ConfigurationsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly Microsoft.Extensions.Hosting.IHostApplicationLifetime lifetime;

        public ConfigurationsController(IConfiguration configuration, Microsoft.Extensions.Hosting.IHostApplicationLifetime lifetime)
        {
            this.configuration = configuration;
            this.lifetime = lifetime;
        }

        [HttpOptions("reloadConfig")]
        public IActionResult ReloadConfiguration()
        {
            try
            {
                var config = (IConfigurationRoot)configuration;
                config.Reload();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpOptions("shutdownapp")]
        public IActionResult ShutdownApp()
        {
            try
            {
                lifetime.StopApplication();

                return Ok("App is restarting...");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
