using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.AnalyticsViewModels;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes;

public class AnalyticService : IAnalyticService
{
    private readonly IUnitOfWork _unitOfWork;

    public AnalyticService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AnalyticsViewModel> GetAnalyticsAsync(CancellationToken ct)
    {
        var model = new AnalyticsViewModel();

        model.TotalMembers = await _unitOfWork.GetRepository<Member>().CountAsync(ct: ct);
        model.ActiveMembers = await _unitOfWork.GetRepository<Membership>().CountAsync(ct: ct);
        model.TotalTrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync(ct: ct);
        model.UpcomingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(S => S.StartDate > DateTime.Now , ct);
        model.OngoingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(S => S.StartDate <= DateTime.Now && S.EndDate > DateTime.Now, ct);
        model.CompletedSessions = await _unitOfWork.GetRepository<Session>().CountAsync(S => S.EndDate <= DateTime.Now, ct);

        return model;
    }
}
