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
     
        public IActionResult Register([FromBody] RequestMusiciansJson register)
        {
            
            var useCase = new RegisterUseCaseJson(music);
            var response = useCase.Execute(register);
            
            var usecaseRegister = new RegisterUseCaseJson(music).ExecuteRegister(register);

   
            return Created(string.Empty, usecaseRegister);


        }


        // Visualização
        [HttpGet]
        [ProducesResponseType(typeof(MusicianRepository), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseNotFoundJson), StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {

            var useCase = new GetAllUseCaseJson(music);
            var response = useCase.Execute();

            if (response == null || response.Count == 0)
            {
                return NotFound(new ResponseNotFoundJson { ListNotFound = "List Not Found, Try Again !" });
            }


            return Ok(response);

        }

        // Visualização por ID
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseMusicJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseNotFoundJson), StatusCodes.Status400BadRequest)]
        public IActionResult GetById(int id)
        {


            var useCase = new GetByIDUseCaseJson(music);
            var response = useCase.Execute(id);

            if (response == null)
            {
                return NotFound(new ResponseNotFoundJson { ListNotFound = "List Not Found, Try Again !" });
            }

            return Ok(response);
        }



        // Atualização por ID
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseNotFoundJson), StatusCodes.Status400BadRequest)]
        public IActionResult Update(int id, RequestUpdateMusiciansJson update)
        {
            var isThere = music.GetById(id);
            if (isThere == null)
            {
                return NotFound(new ResponseNotFoundJson { ListNotFound = "List Not Found, Try Again !" });
            }


            var useCase = new UpdateUseCaseJson(music);
            var response = useCase.Execute(id, update);


            return NoContent();

        }


        // Delete por ID
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseNotFoundJson), StatusCodes.Status400BadRequest)]
        public IActionResult Delete(int id)
        {
            var useCase = new DeleteUseCaseJson(music);
            var response = useCase.Execute(id);

            if (response)
            {
                return NoContent();
            }
            else
            {
                return NotFound(new ResponseNotFoundJson { ListNotFound = "List Not Found, Try Again !"});
            }
        }
    }
}
