using GymManagement.BLL.Services.Classes;
using GymManagement.DAL.Models;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Repositories.Classes;

public class SessionRepository : GenericRepository<Session>, ISessionRepository
{
    private readonly GymDbContext _context;

    public SessionRepository(GymDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct)
       => await _context.Sessions.AsNoTracking()
                .Include(S => S.Trainer)
                .Include(S => S.Category)
                .ToListAsync(ct);

    public async Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default)
       => await _context.Bookings
                .AsNoTracking()
                .CountAsync(B => B.SessionId == sessionId, ct);

    public async Task<Session?> GetSessionByIdWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct = default)
        => await _context.Sessions.AsNoTracking()
                 .Include(S => S.Trainer)
                 .Include(S => S.Category)
                 .FirstOrDefaultAsync(S => S.Id == sessionId, ct);
}
