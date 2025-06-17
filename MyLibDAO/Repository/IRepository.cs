using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibDAO.Repository
{
    internal interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetFiltered(string query);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Remove(int entityId);
    }
}
