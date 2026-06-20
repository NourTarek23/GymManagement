using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Trainers;
using GymManagement.DAL;
using GymManagement.DAL.Models;

namespace GymManagement.BLL.Services.Classes;

public class TrainerService : ITrainerService
{
    private readonly IUnitOfWork _unitOfWork;

    public TrainerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Trainer>> GetAllTrainersAsync(CancellationToken ct)
    {
        var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);

        return trainers;
    }

    public async Task<Trainer?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct)
    {
        var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);

        if (trainer is null) return null;

        return trainer;
    }

    public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct)
    {
        var emailExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(T => T.Email == model.Email, ct);
        var phoneExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(T => T.Phone == model.Phone, ct);

        if (emailExist || phoneExist) return false;

        var trainer = new Trainer()
        {
            Name = model.Name,
            Phone = model.Phone,
            Email = model.Email,
            Gender = model.Gender,
            DateOfBirth = model.DateOfBirth,
            Specialty = model.Specialty,
            Address = new Address()
            {
                BuildingNumber = model.BuildingNumber,
                Street = model.Street,
                City = model.City
            }
        };


        _unitOfWork.GetRepository<Trainer>().Add(trainer);

        var count = await _unitOfWork.SaveChangesAsync(ct);

        return count > 0;
    }


    public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct)
    {
        var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
        if(trainer is null) return null;

        var model = new TrainerToUpdateViewModel()
        {
            Name = trainer.Name,
            Phone = trainer.Phone,
            Email = trainer.Email,
            BuildingNumber = trainer.Address.BuildingNumber,
            Street = trainer.Address.Street,
            City = trainer.Address.City,
            Specialty = trainer.Specialty
        };

        return model;
    }


    public async Task<bool> UpdateTrainerAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct)
    {
        var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
        if(trainer is null) return false;

        var emailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(T => T.Email == model.Email && T.Id != trainerId, ct);
        var phoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(T => T.Phone == model.Phone && T.Id != trainerId, ct);

        if (emailExists || phoneExists) return false;

        trainer.Email = model.Email;
        trainer.Phone = model.Phone;
        trainer.Specialty = model.Specialty;
        trainer.Address.BuildingNumber = model.BuildingNumber;
        trainer.Address.Street = model.Street;
        trainer.Address.City = model.City;

        _unitOfWork.GetRepository<Trainer>().Update(trainer);

        var count = await _unitOfWork.SaveChangesAsync(ct);

        return count > 0;
    }

    public async Task<bool> DeleteTrainerAsync(int trainerId, CancellationToken ct)
    {
        var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
        if(trainer is null) return false;

        _unitOfWork.GetRepository<Trainer>().Delete(trainer);

        var count = await _unitOfWork.SaveChangesAsync(ct);

        return (count > 0);
    }

   
}
