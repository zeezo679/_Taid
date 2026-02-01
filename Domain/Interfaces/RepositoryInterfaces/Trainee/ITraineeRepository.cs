namespace Core.Models.Interfaces.Trainee
{
    public interface ITraineeRepository
    {
        List<Web.Models.Entities.Trainee> Load(bool ordered);
        Web.Models.Entities.Trainee Get(int id);
        bool IsAlreadyTrainee(string UID);
        void Insert(Web.Models.Entities.Trainee trainee);
        void InsertTraineeCrsResult(Web.Models.Entities.Trainee trainee, int crsId);
        void Update(int id, Web.Models.Entities.Trainee trainee);
        Task DeleteAsync(int id);
    }
}
