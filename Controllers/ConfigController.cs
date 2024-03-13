using ManagerApi.Model;
using ManagerIO.Business;
using ManagerIO.Business.Interfaces;
using ManagerIO.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;

namespace ManagerIO.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    //[Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]

    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;
        private IConfigBusiness _configBusiness;

        public ConfigController(ILogger<ConfigController> logger, IConfigBusiness configBusiness)
        {
            _logger = logger;
            _configBusiness = configBusiness;
        }

        [HttpGet]
        public IActionResult CheckConnection()
        {
            Console.WriteLine(DateTime.Now.ToString("T") + " - [ CHECK ] - [ Teste de comunicação ]");
            return Ok("Connect");
        }
        [HttpGet]
        [Route("TokenCheck")]
        public IActionResult TokenCheck()
        {
            Console.WriteLine(DateTime.Now.ToString("T") + " - [ CHECK ] - [ Teste de Token ]");
            return Ok(true);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            Console.WriteLine(DateTime.Now.ToString("T") + " - [ GET ] - [ Config ]");
            return Ok(_configBusiness.FindAll());
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Put([FromBody] Config config)
        {
            Console.WriteLine(DateTime.Now.ToString("T") + " - [ UPDATE ] - [ Config ]");

            if (config == null) return BadRequest();

            return Ok(_configBusiness.Update(config));
        }
    }
}
