using Microsoft.EntityFrameworkCore;
using MyLibDAO.Model;

namespace MyLibDAO.Repository
{
    public class UserRepository : IRepository<User>
    {
        private LibraryContext _context;

        public UserRepository(LibraryContext context)
        {
            _context = context;
        }
        
        public Task<IEnumerable<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetFiltered(string credentials)
        {
            try
            {
                string[] parts = credentials.Split(' ');
                string username = parts[0];
                string password = parts[1];

                return await _context.Users.Where(u =>
                    u.Username == username && u.Password == password
                    ).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Add(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(User entity)
        {
            throw new NotImplementedException();
        }
                
        public async Task<bool> Remove(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
