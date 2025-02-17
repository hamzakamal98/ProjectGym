using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> ListAsync(); 
        Task<T?> GetByIdAsync(int id); 
        Task<T> SaveAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);

    }
}
