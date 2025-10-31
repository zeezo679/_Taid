using Demo.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Demo.Models.Interfaces
{
    public interface IDepartmentRepository
    {
        List<Department> Load();
        Department Get(int id);
        void Insert(Department department);
        DbSet<Department> LoadDeferred();
        void Update(int id, Department newDepartment);
        List<(string DeptKey, int CourseCount)> CountCoursesPerDepartment();
        void Delete(int id);
    }
}
