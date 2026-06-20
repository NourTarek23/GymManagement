using GymManagement.BLL.ViewModels.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces;

public interface ISessionService
{
    Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct);
}
