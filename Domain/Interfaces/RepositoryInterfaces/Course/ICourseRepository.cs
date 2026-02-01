using Web.Models.Entities;

namespace Web.Models.Interfaces
{
    public interface ICourseRepository
    {
         List<Course> Load();
         Course Get(int id);
         void Insert(Course course);
        
         List<Course> LoadRecent();

         List<Course> FilterByDept(int? deptId);
         void Update(int id, Course newCourse);
         void Delete(int id);
         
         int Count();
    }
}
