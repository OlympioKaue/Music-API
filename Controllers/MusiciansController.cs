using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Musicians.Application.UseCase.Musicians.Delete;
using Musicians.Application.UseCase.Musicians.GetAll;
using Musicians.Application.UseCase.Musicians.GetByID;
using Musicians.Application.UseCase.Musicians.Register;
using Musicians.Application.UseCase.Musicians.Update;
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

        // Private indicando privacidade e segurança da classe.
        // Readonly indicando imutabilidade após a inicialização da lista.


        //Criação 
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RequestMusiciansJson register)
        {
            var UseCase = new RegisterUseCaseJson(music);
            var Response = UseCase.Execute(register);

            return Created(string.Empty, Response);


        }


        // Visualização
        [HttpGet]
        [ProducesResponseType(typeof(MusicianRepository), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {

            var UseCase = new GetAllUseCaseJson(music);
            var Response = UseCase.Execute();

            if (Response == null || Response.Count == 0)
            {
                return NotFound(new ResponseErrorJson { Errors = "Failed" });
            }


            return Ok(Response);

        }

        // Visualização por ID
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(MusicianRepository), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public IActionResult GetById(int id)
        {


            var UseCase = new GetByIDUseCaseJson(music);
            var Response = UseCase.Execute(id);

            if (Response == null)
            {
                return NoContent();
            }

            return Ok(Response);
        }



        // Atualização por ID
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public IActionResult Update(int id, RequestMusiciansJson update)
        {
            var exist = music.GetById(id);
            if (exist == null)
            {
                return NotFound(new ResponseErrorJson { Errors = "Failed" });
            }


            var UseCase = new UpdateUseCaseJson(music);
            var Response = UseCase.Execute(id, update);


            return NoContent();

        }


        // Delete por ID
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public IActionResult Delete(int id)
        {
            var UseCase = new DeleteUseCaseJson(music);
            var Response = UseCase.Execute(id);

            if (Response)
            {
                return NoContent();
            }
            else
            {
                return NotFound(new ResponseErrorJson { Errors = "Failed" });
            }
        }
    }
}
