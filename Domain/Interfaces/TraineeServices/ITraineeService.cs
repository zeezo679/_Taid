using Web.ViewModel;

namespace Core.Models.Interfaces.Trainee;

public interface ITraineeService
{
    Task<bool> AddTraineeAsync(TraineeViewModel newTrainee);
}