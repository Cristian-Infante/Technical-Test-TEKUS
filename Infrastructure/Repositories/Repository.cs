using System.Linq.Expressions;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Repository<T>(AppDbContext context) : IRepository<T>
        where T : class
    {
        protected readonly AppDbContext Context = context;
        private readonly DbSet<T> _entities = context.Set<T>();

        public virtual T GetById(int id) => _entities.Find(id)!;

        public virtual IEnumerable<T> GetAll() => _entities.ToList();

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) 
            => _entities.Where(predicate).ToList();

        public void Add(T entity) => _entities.Add(entity);

        public void Update(T entity) => _entities.Update(entity);

        public void Delete(T entity) => _entities.Remove(entity);
    }
}