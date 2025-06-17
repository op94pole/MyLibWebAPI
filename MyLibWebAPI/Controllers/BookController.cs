using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyLibDAO.Model;

using MyLibDAO.Repository;

namespace MyLibWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookRepository _bookRepository;
        private readonly ReservationRepository _reservationRepository;

        public BookController(BookRepository bookRepository, ReservationRepository reservationRepository)
        {
            _bookRepository = bookRepository;
            _reservationRepository = reservationRepository;
        }

        [HttpGet(Name = "GetBooks")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(string? query = null)
        {            
            try
            {
                var booksList = string.IsNullOrEmpty(query)
                    ? await _bookRepository.GetAll()
                    : await _bookRepository.GetFiltered(query);                

                if (booksList.IsNullOrEmpty<Book>())
                {
                    return NotFound("Nessun libro trovato! Riprova.");
                }

                return Ok(booksList);
            }
            catch (Exception) 
            {
                return StatusCode(500, "Errore generico! Riprova.");
            }            
        }

        [HttpPost(Name = "PostNewBook")]
        public async Task<ActionResult<string>> PostNewBook(string title, string authorName, string authorSurname, string publisher, int quantity)
        {
            try
            {
                await _bookRepository.Add(new Book
                {
                    Title = title,
                    AuthorName = authorName,
                    AuthorSurname = authorSurname,
                    Publisher = publisher,
                    Quantity = quantity
                });

                return Ok("Libro inserito correttamente.");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Libro già presente a sistema! Riprova.");
            }
            catch (Exception)
            {
                return BadRequest("Campi non valorizzati correttamente! Riprova.");
            }
        }

        [HttpPut(Name = "PutBook")]
        public async Task<ActionResult<string>> PutBook(Book updatedBook)
        {
            try
            {
                if (string.IsNullOrEmpty(updatedBook.Title) ||
                    string.IsNullOrEmpty(updatedBook.AuthorName) ||
                    string.IsNullOrEmpty(updatedBook.AuthorSurname) ||
                    string.IsNullOrEmpty(updatedBook.Publisher) ||
                    updatedBook.Quantity == 0)
                {
                    return BadRequest("Campi non valorizzati correttamente! Riprova");
                }

                await _bookRepository.Update(updatedBook);

                return Ok("Libro aggiornato correttamente.");
            }
            catch
            {
                return StatusCode(500, "Errore generico! Riprova.");
            }            
        }

        [HttpDelete(Name = "DeleteBook")]
        public async Task<ActionResult<string>> DeleteBook(int bookId)
        {
            try
            {
                var reservationsList = await _reservationRepository.GetAll();
                var bookReservations = reservationsList.Where(r => 
                r.BookId == bookId && r.EndDate > DateOnly.FromDateTime(DateTime.UtcNow)
                ).ToList();

                if (bookReservations.Any()) 
                {
                    return BadRequest("Impossibile eliminare il libro! Esistono una o più prenotazioni attive associate. Riprova");
                }

                await _bookRepository.Remove(bookId);

                return Ok("Libro eliminato correttamente.");
            }
            catch
            {
                return StatusCode(500, "Errore generico! Riprova.");
            }
        }
    }
}
