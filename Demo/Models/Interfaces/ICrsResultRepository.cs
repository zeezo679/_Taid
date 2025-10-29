using Demo.Models.Entities;

namespace Demo.Models.Interfaces
{
    public interface ICrsResultRepository
    {
        void Insert(Trainee trainee, int crsId);
        bool CheckEnrollStatus(int crsId, string uid);
        
        //method here called getCoursesofCurrentUser(string uid) that returns a list of courses filtered by course Id then that list is passed to the home controller to be passed to view
        Task<List<CrsResult>> FilterCoursesByCurrentUserAsync(string uid);
        void IsEnrolled(string UID);
    }
}
