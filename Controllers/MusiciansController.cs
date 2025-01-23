using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Musicians.Application.UseCase.Musicians.GetAll;
using Musicians.Application.UseCase.Musicians.Register;
using Musicians.Communication.Repository;
using Musicians.Communication.Request;
using Musicians.Communication.Response;

namespace Musicians.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusiciansController : ControllerBase
    {
        private readonly MusicianRepository music = new MusicianRepository();


        [HttpPost]
        [ProducesResponseType(typeof(ResponseShortJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RequestMusiciansJson register)
        {
            var UseCase = new RegisterUseCaseJson(music);
            var Response = UseCase.Execute(register);  


            return Created(string.Empty, Response);

        }

        [HttpGet]
        [ProducesResponseType(typeof(MusicianRepository), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var UseCase = new GetAllUseCaseJson(music);
            var Response = UseCase.Execute();

            return Ok(Response);
        }
    }
}
