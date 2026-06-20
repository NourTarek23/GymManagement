using GymManagement.BLL.Services.Classes;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL;

public interface IUnitOfWork
{
    IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();

    Task<int> SaveChangesAsync(CancellationToken ct = default);

    public ISessionRepository SessionRepository { get; }

}
