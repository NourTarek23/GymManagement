using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces;

public interface ISessionService
{
    Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default);

    Task<Result<SessionViewModel>> GetSessionByIdAsync(int sessionId, CancellationToken ct = default);

    Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);

    Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersAsync(CancellationToken ct = default);

    Task<IEnumerable<CategorySelectViewModel>> GetAllCategoriesAsync(CancellationToken ct = default);

    Task<Result<SessionToUpdateViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default);

    Task<Result> UpdateSessionAsync(int sessionId, SessionToUpdateViewModel model, CancellationToken ct = default);

    Task<Result> DeleteSessionAsync(int sessionId, CancellationToken ct = default);
}
