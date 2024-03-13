using ManagerApi.DM;
using ManagerApi.Model;
using ManagerIO.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagerIO.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]

    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private IUsersBusiness _userBusiness;

        public UsersController(ILogger<UsersController> logger, IUsersBusiness userBusiness)
        {
            _logger = logger;
            _userBusiness = userBusiness;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userBusiness.FindAll());
        }

        [HttpPost]
        public IActionResult Post([FromBody] UsersDM user)
        {
            if (user == null) return BadRequest();
            ResultDM result = _userBusiness.CreateNewUser(user);

            return Ok(result);
        }
        [HttpPut]
        public IActionResult EditUser([FromBody] UsersDM user)
        {
            if (user == null) return BadRequest();
            ResultDM result = _userBusiness.UpdateUser(user);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (id == null) return BadRequest();
            ResultDM result = _userBusiness.RemoveUser(id);

            return Ok(result);
        }
    }
}
