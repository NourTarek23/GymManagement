using GymManagement.BLL.Services.Classes;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;

namespace GymManagement.DAL;

public class UnitOfWork : IUnitOfWork
{
    private readonly GymDbContext _context;
    private readonly ISessionRepository _sessionRepository;
    private readonly Dictionary<string, object> _repositories = [];



    public UnitOfWork(GymDbContext context, ISessionRepository sessionRepository)
    {
        _context = context;
        _sessionRepository = sessionRepository;
    }

    public ISessionRepository SessionRepository => _sessionRepository;

    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
    {
        //Generate Repository of TEntity
        //check TEntity if Exists or not 

        var typeName = typeof(TEntity).Name;

        if (_repositories.TryGetValue(typeName, out object? value))
            return value as IGenericRepository<TEntity>;

        var repository = new GenericRepository<TEntity>(_context);

        _repositories.Add(typeName, repository);

        return repository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}
