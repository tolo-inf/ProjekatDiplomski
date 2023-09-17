using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjekatDiplomski.Helper;
using ProjekatDiplomski.Models;
using ProjekatDiplomski.RequestModels;
using ProjekatDiplomski.Services.IServices;

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

        [Route("AddGame")]
        [HttpPost]
        public async Task<ActionResult> AddGame([FromForm] RequestGame game, IFormFile img)
        {
            try
            {
                string imagePath = await ImageHelper.SaveImage(img);
                var result = _gameService.AddGame(imagePath, game.Name, game.Description, game.Developer, game.Publisher, game.Genres, game.Systems, game.Year, game.Price, game.Rating);

                return Ok("Game successfully added!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("ReplaceGame")]
        [HttpPost]
        public async Task<ActionResult> ReplaceGame([FromForm] Game game)
        {
            try
            {
                var result = await _gameService.ReplaceGame(game.Id, game.Image, game.Name, game.Description, game.Developer, game.Publisher, game.Genres, game.Systems, game.Year, game.Price, game.Rating);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DeleteGame")]
        [HttpDelete]
        public async Task<ActionResult> DeleteGame(long id)
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

        [Route("GetGameById")]
        [HttpGet]
        public async Task<ActionResult> GetGameById(long id)
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

        [Route("PerformSearch")]
        [HttpPost]
        public async Task<ActionResult> PerformSearch([FromForm] SearchGame game)
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
