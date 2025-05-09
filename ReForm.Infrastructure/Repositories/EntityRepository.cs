using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ReForm.Core.Interfaces;

namespace ReForm.Infrastructure.Repositories;

public class EntityRepository<T>(ApplicationDbContext context) : IEntityRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.FirstOrDefaultAsync(predicate);

    public async Task<T?> FirstOrDefaultAsyncWithIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public IQueryable<T> AsQueryable()
    {
        return _dbSet.AsQueryable();
    }
}