using ChefApp.Models;
using Microsoft.EntityFrameworkCore;

namespace SmileChef.Repository
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly ChefAppContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ChefAppContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual List<T>? GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual async Task<List<T>>? GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual T? GetById(int id)
        {
            T? foundEntity = _dbSet.Find(id);
            if (foundEntity == null)
            {
                throw new Exception($"We couldn't find any entity with id: {id}");
            }
            return foundEntity;
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
