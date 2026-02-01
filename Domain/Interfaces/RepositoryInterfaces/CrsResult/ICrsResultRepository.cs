using Web.Models.Entities;
using Web.ViewModel;

namespace Web.Models.Interfaces
{
    public interface ICrsResultRepository
    {
        void Insert(Trainee trainee, int crsId);

        List<CrsResult> Load();

        List<EnrollmentViewModel> GetRecentEnrollments();
        
        bool CheckEnrollStatus(int crsId, string uid);
        
        //method here called getCoursesofCurrentUser(string uid) that returns a list of courses filtered by course Id then that list is passed to the home controller to be passed to view
        Task<List<CrsResult>> FilterCoursesByCurrentUserAsync(string uid);
        void IsEnrolled(string UID);
    }
}
