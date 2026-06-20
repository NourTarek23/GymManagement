using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.BLL.Services.Classes;

public interface ISessionRepository : IGenericRepository<Session>
{
    Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct = default);

    Task<int> GetCountOfBookedSlotsAsync(int SessionId, CancellationToken ct = default);
}
