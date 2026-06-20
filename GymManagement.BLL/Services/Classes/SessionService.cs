using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Sessions;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes;

public class SessionService : ISessionService
{
    private readonly IUnitOfWork _unitOfWork;

    public SessionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct)
    {
        var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct: ct);
        if (sessions is null || !sessions.Any()) return null;

        var models = sessions.Select(S => new SessionViewModel()
        {
            Id = S.Id,
            Capacity = S.Capacity,
            CategoryName = S.Category.CategoryName,
            TrainerName = S.Trainer.Name,
            Description = S.Description,
            EndDate = S.EndDate,
            StartDate = S.StartDate
        });

        foreach (var session in models)
        {
            session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
        }

        return models;
    }
}
