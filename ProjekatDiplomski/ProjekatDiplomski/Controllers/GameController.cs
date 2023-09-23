using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjekatDiplomski.Helper;
using ProjekatDiplomski.Models;
using ProjekatDiplomski.RequestModels;
using ProjekatDiplomski.Services;
using ProjekatDiplomski.Services.IServices;
using System.Xml.Linq;

namespace ProjekatDiplomski.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GameController(IGameService gameService) 
        {
            _gameService = gameService;
        }

        [AllowAnonymous]
        [Route("SaveImage")]
        [HttpPost]
        public async Task<ActionResult> SaveImage(IFormFile img)
        {
            try
            {
                string imagePath = await ImageHelper.SaveImage(img);
                
                if (String.IsNullOrWhiteSpace(imagePath))
                {
                    return BadRequest($"Image failed to save!");
                }

                return Ok(imagePath);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("AddGame")]
        [HttpPost]
        public async Task<ActionResult> AddGame([FromBody] RequestGame game)
        {
            try
            {
                var result = _gameService.AddGame(game.Image, game.Name, game.Description, game.Developer, game.Publisher, game.Genres, game.Systems, game.Year, game.Price, game.Rating);

                if (result.Result == null) 
                {
                    return BadRequest($"Game with name:{game.Name} already exist!");
                }

                return Ok($"Game with name:{game.Name} successfully added!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("ReplaceGame")]
        [HttpPost]
        public async Task<ActionResult> ReplaceGame([FromBody] RequestGame game)
        {
            try
            {
                var result = await _gameService.ReplaceGame(game.Image, game.Name, game.Description, game.Developer, game.Publisher, game.Genres, game.Systems, game.Year, game.Price, game.Rating);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("DeleteGameById")]
        [HttpDelete]
        public async Task<ActionResult> DeleteGame(ulong id)
        {
            try
            {
                var result = await _gameService.DeleteGame(id);
                return Ok("Game successfully deleted!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("DeleteGame")]
        [HttpDelete]
        public async Task<ActionResult> DeleteGame(string name)
        {
            try
            {
                var game = await _gameService.GetGameByName(name);
                var result = await _gameService.DeleteGame(game.Id);
                return Ok("Game successfully deleted!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("GetGameById")]
        [HttpGet]
        public async Task<ActionResult> GetGameById(ulong id)
        {
            try
            {
                var result = await _gameService.GetGameById(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("GetGameByName")]
        [HttpGet]
        public async Task<ActionResult> GetGameByName(string name)
        {
            try
            {
                var result = await _gameService.GetGameByName(name);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("GetAllGames")]
        [HttpGet]
        public async Task<ActionResult> GetAllGames()
        {
            try
            {
                var result = await _gameService.GetAllGames();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("PerformSearch")]
        [HttpPost]
        public async Task<ActionResult> PerformSearch([FromBody] SearchGame game)
        {
            try
            {
                var result = await _gameService.PerformSearch(game.Name, game.Description, game.Developer, game.Publisher, game.Genres, game.Systems, game.YearStart, game.YearEnd, game.PriceStart, game.PriceEnd, game.RatingStart, game.RatingEnd);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
