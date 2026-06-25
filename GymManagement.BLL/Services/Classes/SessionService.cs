using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Sessions;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enums;

namespace GymManagement.BLL.Services.Classes;

public class SessionService : ISessionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SessionService(IUnitOfWork unitOfWork,
                          IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct)
    {
        var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct: ct);
        if (sessions is null || !sessions.Any()) return null;

        var models = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

        foreach (var session in models)
        {
            session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
        }

        return models;
    }

    public async Task<Result<SessionViewModel>> GetSessionByIdAsync(int sessionId, CancellationToken ct = default)
    {
        var session = await _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategoryAsync(sessionId, ct);

        if (session is null) return Result<SessionViewModel>.NotFound($"Session With Id {sessionId} Not Found");

        var model = _mapper.Map<SessionViewModel>(session);

        model.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);

        return Result<SessionViewModel>.Ok(model);
    }


    public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct)
    {
        if (model.EndDate <= model.StartDate) return Result.Validation("EndDate Must Be After StartDate");
        if (model.StartDate <= DateTime.Now) return Result.Validation("StartDate Must Be In The Future");
        if (model.Capacity < 1 || model.Capacity > 25) return Result.Validation("Capacity Must Be Between 1 and 25");

        var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
        if (trainer is null) return Result.NotFound($"Trainer With Id {model.TrainerId} Not Found");

        var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId, ct);
        if (category is null) return Result.NotFound($"Category With Id {model.CategoryId} Not Found");

        var isValid = Enum.TryParse<Specialty>(category.CategoryName, out var categorySpecialty);
        if (!isValid || trainer.Specialty != categorySpecialty) return Result.Validation("Can't Create Session with This Trainer Because Of His Specialty");

        var session = _mapper.Map<Session>(model);

        _unitOfWork.GetRepository<Session>().Add(session);
        var count = await _unitOfWork.SaveChangesAsync(ct);
    
         
        return count > 0 ? Result.Ok() : Result.Fail("Failed To Create Session");
    }

    public async Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
    {
        var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);

        var models = _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);

        return models;
    }

    public async Task<IEnumerable<CategorySelectViewModel>> GetAllCategoriesAsync(CancellationToken ct = default)
    {
        var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(ct: ct);

        var models = _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);

        return models;
    }


    public async Task<Result<SessionToUpdateViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct)
    {
        var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(sessionId, ct);

        if(session is null) return Result<SessionToUpdateViewModel>.NotFound($"Session with id {sessionId} not found");

        if (session.StartDate <= DateTime.Now) return Result<SessionToUpdateViewModel>.Fail("can not update session with outgoing or completed status");

        var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);
        if(bookingCount > 0) return Result<SessionToUpdateViewModel>.Fail("can not update session that has already bookings");

        var model = _mapper.Map<SessionToUpdateViewModel>(session);

        return Result<SessionToUpdateViewModel>.Ok(model);
    }

    public async Task<Result> UpdateSessionAsync(int sessionId, SessionToUpdateViewModel model, CancellationToken ct = default)
    {
        var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(sessionId, ct);

        if (session is null) return Result.NotFound($"Session with id {sessionId} not found");

        if (model.StartDate <= DateTime.Now) return Result.Validation("StartDate must be in the future");

        if (model.EndDate <= model.StartDate) return Result.Validation("EndDate must be after StartDate");

        var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
        if (trainer is null) return Result.NotFound($"Trainer With Id {model.TrainerId} Not Found");

        var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId, ct);
        if (category is null) return Result.NotFound($"Category With Id {session.CategoryId} Not Found");

        var isValid = Enum.TryParse<Specialty>(category.CategoryName, out var categorySpecialty);
        if (!isValid || trainer.Specialty != categorySpecialty) return Result.Validation("Can't Update Session with This Trainer Because His Specialty doesn't match Category");

        session.StartDate = model.StartDate;
        session.EndDate = model.EndDate;
        session.Description = model.Description;
        session.TrainerId = model.TrainerId;
        session.UpdatedAt = DateTime.Now;

        _unitOfWork.SessionRepository.Update(session);

        var count = await _unitOfWork.SaveChangesAsync(ct);

        return count > 0 ? Result.Ok() : Result.Fail("Failed To Update Session");
    }

    public async Task<Result> DeleteSessionAsync(int sessionId, CancellationToken ct = default)
    {
        var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(sessionId, ct);

        if (session is null) return Result.NotFound($"Session with id {sessionId} not found !");

        if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return Result.Fail("Can't Delete Ongoin Session !"); 
        
        _unitOfWork.GetRepository<Session>().Delete(session);
        var count = await _unitOfWork.SaveChangesAsync(ct);

        return count > 0 ? Result.Ok() : Result.Fail("Failed To Delete Session");
    }
}
