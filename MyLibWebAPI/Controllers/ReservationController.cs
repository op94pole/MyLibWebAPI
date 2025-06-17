using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyLibDAO.Model;
using MyLibDAO.Repository;

namespace MyLibWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationRepository _reservationRepository;

        public ReservationController(ReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        [HttpGet(Name = "GetReservations")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations(string? query = null)
        {
            try 
            {
                var reservationsList = string.IsNullOrEmpty(query)
                ? await _reservationRepository.GetAll()
                : await _reservationRepository.GetFiltered(query);

                if (reservationsList.IsNullOrEmpty())
                {
                    return NotFound("Nessuna prenotazione trovata! Riprova.");
                }

                return Ok(reservationsList);
            }
            catch (Exception)
            {
                return StatusCode(500, "Errore generico! Riprova.");
            }
        }

        [HttpPost(Name = "CreateReservation")]
        public async Task<ActionResult<string>> CreateReservation(int userId, int bookId)
        {
            try
            {
                await _reservationRepository.Add(new Reservation
                {
                    UserId = userId,
                    BookId = bookId,
                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30))
                });
            }
            catch (Exception)
            {
                return BadRequest("Impossibile concludere la prenotazione! Possiedi già questo libro in prestito.");
            }
            
            return Ok("Libro prenotato con successo.");
        }

        [HttpPut(Name = "UpdateReservation")]
        public async Task<ActionResult<string>> UpdateReservation(Reservation updatedReservation)
        {
            try
            {
                await _reservationRepository.Update(updatedReservation);

                return Ok("Prenotazione terminata con successo.");
            }
            catch
            {
                return StatusCode(500, "Errore generico! Riprova.");
            }            
        }
    }
}
