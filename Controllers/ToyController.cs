
using ManagerApi.Business;
using ManagerApi.Business.Interfaces;
using ManagerApi.Model;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;

namespace ManagerApi.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    //[Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class ToyController : ControllerBase
    {
        private readonly ILogger<ToyController> _logger;
        private IToyBusiness _toyBusiness;


        public ToyController(ILogger<ToyController> logger, IToyBusiness toyBusiness)
        {
            _logger = logger;
            _toyBusiness = toyBusiness;
        }

        [HttpGet]
        public IActionResult GetAll() 
        {
            return Ok(_toyBusiness.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long Id)
        {
            var toy = _toyBusiness.FindById(Id);

            if (toy == null) return NotFound();

            return Ok(toy);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Toy toy)
        {
            if (toy == null) return BadRequest();

            return Ok(_toyBusiness.Create(toy));
        }

        [HttpPut]
        public IActionResult Put([FromBody] Toy toy)
        {
            if (toy == null) return BadRequest();

            return Ok(_toyBusiness.Update(toy));
        }

    }
}
