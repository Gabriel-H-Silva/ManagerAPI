
using ManagerApi.Business.Interfaces;
using ManagerApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace ManagerApi.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    //[Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class ReadersSettingsController : ControllerBase
    {
        private readonly ILogger<ReadersSettingsController> _logger;
        private IReadersSettingsBusiness _readersSettingsBusiness;


        public ReadersSettingsController(ILogger<ReadersSettingsController> logger, IReadersSettingsBusiness readersSettingsBusiness)
        {
            _logger = logger;
            _readersSettingsBusiness = readersSettingsBusiness;
        }

        [HttpGet]
        public IActionResult GetAll() 
        {
            return Ok(_readersSettingsBusiness.GetAll());
        }

        [HttpGet]
        [Route("ativos")]
        public IActionResult GetAtivos()
        {
            return Ok(_readersSettingsBusiness.GetAtivos());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long Id)
        {
            var reader = _readersSettingsBusiness.FindById(Id);

            if (reader == null) return NotFound();

            return Ok(reader);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ReadersSettings readers_settings)
        {
            if (readers_settings == null) return BadRequest();

            return Ok(_readersSettingsBusiness.Create(readers_settings));
        }

        [HttpPut]
        public IActionResult Put([FromBody] ReadersSettings readers_settings)
        {
            if (readers_settings == null) return BadRequest();

            return Ok(_readersSettingsBusiness.Update(readers_settings));
        }

        [HttpPost]
        [Route("reativacaostatus\"{id}")]
        public IActionResult Reativacao(long id)
        {
            var readers_settings = _readersSettingsBusiness.FindById(id);

            if (readers_settings == null) return BadRequest();

            _readersSettingsBusiness.ReativarStatus(id);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _readersSettingsBusiness.DeleteStatus(id);

            return NoContent();
        }
    }
}
