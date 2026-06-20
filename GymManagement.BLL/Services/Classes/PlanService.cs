using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Plans;
using GymManagement.DAL;
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
    private readonly IUnitOfWork _unitOfWork;

    public PlanService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Plan>> GetAllPlansAsync(CancellationToken ct)
    {
        var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);

        return plans;
    }

    public async Task<Plan?> GetPlanDetailsAsync(int planId, CancellationToken ct)
    {
        var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
        if (plan is null) return null;

        return plan;
    }

    public async Task<PlanToUpdateViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct)
    {
        var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
        if (plan is null) return null;

        var model = new PlanToUpdateViewModel()
        {
            Name = plan.Name,
            Price = plan.Price,
            Description = plan.Description,
            Duration = plan.DurationDays
        };

        return model;
    }

    public async Task<bool> UpdatePlanAsync(int planId, PlanToUpdateViewModel model, CancellationToken ct)
    {
        var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);

        if (plan is null) return false;

        // can't Edit Plan Name
        if (model.Name != plan.Name) return false;

        //can't update plan with active membership
        var activeMembership = await _unitOfWork.GetRepository<Membership>().AnyAsync(M => M.PlanId == planId && M.EndDate > DateTime.UtcNow, ct);
        if (activeMembership) return false;

        plan.Price = model.Price;
        plan.Description = model.Description;
        plan.DurationDays = model.Duration;

        _unitOfWork.GetRepository<Plan>().Update(plan);

        var count = await _unitOfWork.SaveChangesAsync(ct);

        return count > 0;
    }

    public async Task<bool> UpdatePlanStatusAsync(int id, CancellationToken ct)
    {
        var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);

        if (plan is null) return false;

        //can't Deactivate plan with active memberships
        if (plan.IsActive && await _unitOfWork.GetRepository<Membership>().AnyAsync(M => M.PlanId == id && M.EndDate > DateTime.UtcNow, ct))
        {
            return false;
        }

        plan.IsActive = !plan.IsActive;

        _unitOfWork.GetRepository<Plan>().Update(plan);

        var count = await _unitOfWork.SaveChangesAsync(ct);

        return (count > 0);
    }
}
