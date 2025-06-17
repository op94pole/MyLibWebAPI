using Microsoft.AspNetCore.Mvc;
using MyLibDAO.Model;
using MyLibDAO.Repository;

namespace MyLibWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet(Name = "GetCurrentUser")]
        public async Task<ActionResult<User?>> GetCurrentUser(string credentials)
        {
            try
            {
                var currentUser = await _userRepository.GetFiltered(credentials);

                if (currentUser == null)
                {
                    return NotFound("Username o Password errati!");
                }

                Response.Headers["success-message"] = $"Benvenuto/a {currentUser.FirstOrDefault().Username}.";

                return Ok(currentUser); 
            }
            catch(Exception) 
            {
                return StatusCode(500, "Errore generico! Riprova.");
            }
        }
    }
}
