using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes;

public class PlanService : IPlanService
{
    private readonly IGenericRepository<Plan> _planRepository;
    private readonly IGenericRepository<Membership> _membershipRepository;

    public PlanService(IGenericRepository<Plan> planRepository,
                       IGenericRepository<Membership> membershipRepository )
    {
        _planRepository = planRepository;
        _membershipRepository = membershipRepository;
    }

    public async Task<IEnumerable<Plan>> GetAllPlansAsync(CancellationToken ct)
    {
        var plans = await _planRepository.GetAllAsync(ct: ct);

        return plans;
    }

    public async Task<Plan?> GetPlanDetailsAsync(int planId, CancellationToken ct)
    {
        var plan = await _planRepository.GetByIdAsync(planId, ct);
        if (plan is null) return null;

        return plan;
    }

    public async Task<bool> UpdatePlanAsync(int planId, Plan plan, CancellationToken ct)
    {
        var model = await _planRepository.GetByIdAsync(planId, ct);

        if (model is null) return false;

        // can't Edit Plan Name
        if (model.Name != plan.Name) return false;

        var count = await _planRepository.UpdateAsync(plan, ct);

        return count > 0;
    }

    public async Task<bool> UpdatePlanStatusAsync(int id, CancellationToken ct)
    {
        var plan = await _planRepository.GetByIdAsync(id, ct);

        if (plan is null) return false;

        if (plan.IsActive && await _membershipRepository.AnyAsync(M => M.PlanId == id && M.EndDate > DateTime.UtcNow, ct))
        {
            return false;
        }

        plan.IsActive = !plan.IsActive;

        var count = await _planRepository.UpdateAsync(plan, ct);

        return (count > 0);
    }
}
