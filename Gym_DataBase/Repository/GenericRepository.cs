using Gym_DataBase.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly AppDbContext _db;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var data = await _dbSet.FindAsync(id);
                if (data == null) return false;

                _dbSet.Remove(data);
                return await _db.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> ListAsync()
        {

            return await _dbSet.ToListAsync();
        }

        public async Task<T> SaveAsync(T entity)
        {
            try
            {
                 _dbSet.Add(entity);
                await _db.SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {

                _dbSet.Update(entity);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
