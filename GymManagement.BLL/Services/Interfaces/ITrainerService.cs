using GymManagement.BLL.ViewModels.Trainers;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces;

public interface ITrainerService
{
    Task<IEnumerable<Trainer>> GetAllTrainersAsync(CancellationToken ct);

    Task<Trainer?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct);

    Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct);

    Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct);

    Task<bool> UpdateTrainerAsync(int trainerId, TrainerToUpdateViewModel trainer, CancellationToken ct);


    Task<bool> DeleteTrainerAsync(int trainerId, CancellationToken ct);
}
