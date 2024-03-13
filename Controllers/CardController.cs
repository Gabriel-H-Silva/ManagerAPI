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
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private ICardBusiness _cardBusiness;

        public CardController(ILogger<CardController> logger, ICardBusiness cardBusiness)
        {
            _logger = logger;
            _cardBusiness = cardBusiness;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_cardBusiness.GetAll());
        }

        [HttpGet]
        [Route("ativos")]
        public IActionResult GetAtivos()
        {
            return Ok(_cardBusiness.GetAtivos());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var card = _cardBusiness.FindById(id);

            if (card == null) return NotFound();

            return Ok(card);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Card card)
        {
            if (card == null) return BadRequest();

            return Ok(_cardBusiness.Create(card));
        }


        [HttpPut]
        public IActionResult Put([FromBody] Card card)
        {
            if (card == null) return BadRequest();

            return Ok(_cardBusiness.Update(card));
        }

        [HttpPost]
        [Route("reativacaostatus\"{id}")]
        public IActionResult Reativacao(long id)
        {
            var card = _cardBusiness.FindById(id);

            if (card == null) return BadRequest();

            _cardBusiness.ReativarStatus(id);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _cardBusiness.DeleteStatus(id);

            return NoContent();
        }
    }
}
