using ManagerApi.Business.Interfaces;
using ManagerApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestAPIcurso.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    //[Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private IClientBusiness _clientBusiness;

        public ClientController(ILogger<ClientController> logger, IClientBusiness clientBusiness)
        {
            _logger = logger;
            _clientBusiness = clientBusiness;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_clientBusiness.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long Id)
        {
            var card = _clientBusiness.FindById(Id);

            if (card == null) return NotFound();

            return Ok(card);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Client client)
        {
            if (client == null) return BadRequest();

            return Ok(_clientBusiness.Create(client));
        }

        [HttpPut]
        public IActionResult Put([FromBody] Client client)
        {
            if (client == null) return BadRequest();
            return Ok(_clientBusiness.Update(client));
        }
    }
}
