using Microsoft.EntityFrameworkCore;
using MyLibDAO.Model;

namespace MyLibDAO.Repository
{
    public class BookRepository : IRepository<Book>
    {
        private LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            try
            {
                return await _context.Books.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Book>> GetFiltered(string query)
        {
            try
            {
                return await _context.Books.Where(b =>
                    b.Title.ToLower().Contains(query.ToLower()) ||
                    b.AuthorName.ToLower().Contains(query.ToLower()) ||
                    b.AuthorSurname.ToLower().Contains(query.ToLower()) ||
                    b.Publisher.ToLower().Contains(query.ToLower())
                    ).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Add(Book newBook)
        {
            try
            {
                await _context.Books.AddAsync(newBook);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(Book bookToUpdate)
        {
            try
            {
                _context.Books.Update(bookToUpdate);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
                
        public async Task<bool> Remove(int bookToRemoveId)
        {
            try
            {
                _context.Books.RemoveRange(_context.Books.Where(b =>
                    b.BookId == bookToRemoveId));
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
