using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{

    private readonly GymDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(GymDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        => tracking ? await _dbSet.ToListAsync(ct) :await _dbSet.AsNoTracking().ToListAsync(ct);
    

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _dbSet.FindAsync(id, ct);
    

    public async Task<int> AddAsync(TEntity entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct);
    }

    public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        _dbSet.Update(entity);
        return await _context.SaveChangesAsync(ct);
    }

    public async Task<int> DeleteAsync(TEntity entity, CancellationToken ct = default)
    {
        _dbSet.Remove(entity);
        return await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
    {
       return await _dbSet.AnyAsync(predicate, ct);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, ct);
    }
}
