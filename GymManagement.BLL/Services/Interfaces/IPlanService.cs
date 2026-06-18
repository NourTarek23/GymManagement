using GymManagement.BLL.ViewModels.Plans;
using GymManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces;

public interface IPlanService
{
    Task<IEnumerable<Plan>> GetAllPlansAsync(CancellationToken ct);

    Task<Plan?> GetPlanDetailsAsync(int planId, CancellationToken ct);

    Task<PlanToUpdateViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct);

    Task<bool> UpdatePlanAsync(int planId, PlanToUpdateViewModel plan, CancellationToken ct);

    Task<bool> UpdatePlanStatusAsync(int id, CancellationToken ct);
}
