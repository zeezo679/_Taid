using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Models.Entities;
using Web.ViewModel;

namespace Web.Models.Interfaces
{
    public interface IInstructorRepository
    {
        List<Instructor> Load();
        List<SelectListItem> LoadSelectItems();
        List<Instructor> LoadInstructorsWithTheirCourses(CourseViewModel CourseView, bool save);
        Task<List<Course>> FilterCoursesByCurrentInstructorAsync(string uid);
        Instructor Get(int id);
        void Insert(Instructor instructor);
        void Update(int id, Instructor newInstructor);
        void Update(Instructor oldInstructor, Instructor newInstructor);
        void Delete(int id);
    }
}
