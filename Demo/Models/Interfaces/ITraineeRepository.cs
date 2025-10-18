using Demo.Models.Entities;

namespace Demo.Models.Interfaces
{
    public interface ITraineeRepository
    {
        List<Trainee> Load(bool ordered);
        Trainee Get(int id);
        bool IsAlreadyTrainee(string UID);
        void Insert(Trainee trainee);
        void InsertTraineeCrsResult(Trainee trainee, int crsId);
        void Update(int id, Trainee trainee);
        void Delete(int id);
    }
}
