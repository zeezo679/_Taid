using Demo.Models.Entities;

namespace Demo.Models.Interfaces
{
    public interface ICrsResultRepository
    {
        void Insert(Trainee trainee, int crsId);
        bool CheckEnrollStatus(int crsId, string uid);
        void IsEnrolled(string UID);
    }
}
