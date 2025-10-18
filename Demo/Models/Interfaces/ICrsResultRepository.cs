using Demo.Models.Entities;

namespace Demo.Models.Interfaces
{
    public interface ICrsResultRepository
    {
        void Insert(Trainee trainee, int crsId);
        void IsEnrolled(string UID);
    }
}
