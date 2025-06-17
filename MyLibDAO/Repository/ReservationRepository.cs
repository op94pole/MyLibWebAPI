using Microsoft.EntityFrameworkCore;
using MyLibDAO.Model;

namespace MyLibDAO.Repository
{
    public class ReservationRepository : IRepository<Reservation>
    {
        LibraryContext _context;

        public ReservationRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAll()
        {
            try
            {
                return await _context.Reservations.Include(r =>
                    r.User).Include(r =>
                    r.Book).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Reservation>> GetFiltered(string query) // not working
        {
            try
            {
                query = query.ToLower();

                return await _context.Reservations.Include(r =>
                    r.User).Include(r =>
                    r.Book).Where(r =>
                    r.User.Username.Contains(query.ToLower()) ||
                    r.Book.Title.Contains(query.ToLower()) ||
                    r.Book.AuthorName.Contains(query.ToLower()) ||
                    r.Book.AuthorSurname.Contains(query.ToLower()) ||
                    r.Book.Publisher.Contains(query.ToLower())
                    ).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Add(Reservation entity)
        {
            try
            {
                var existingReservations = (await GetAll()).Where(r =>
                    r.UserId  == entity.UserId &&
                    r.BookId == entity.BookId &&
                    r.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow)
                    ).ToList();

                if (existingReservations.Any())
                {
                    throw new Exception();
                }

                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(Reservation reservationToUpdate)
        {
            try
            {
                reservationToUpdate.EndDate = DateOnly.FromDateTime(DateTime.UtcNow);

                _context.Update(reservationToUpdate);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw;
            }            
        }

        public async Task<bool> Remove(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
