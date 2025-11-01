using Microsoft.EntityFrameworkCore;
using Web.Infrastructure;
using Web.Models.Entities;
using Web.Models.Interfaces;

namespace Web.Models.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private AppDbContext _context = new AppDbContext();

        public DepartmentRepository(AppDbContext context) {
            _context = context;
        }
        public List<Department> Load()
        {
            var departments = _context.Departments.ToList();
            return departments;
        }

        public DbSet<Department> LoadDeferred()
        {
            var departmentsDeferred = _context.Departments;
            return departmentsDeferred;
        }
        public Department Get(int id)
        {
            Department dept = _context.Departments.FirstOrDefault(c => c.Id == id);
            return dept;
        }
        public void Insert(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }
        public void Update(int id, Department newDepartment)
        {
            var oldCrs = Get(id);
            _context.Entry(oldCrs).CurrentValues.SetValues(newDepartment);
            _context.SaveChanges();
        }

        public List<(string DeptKey, int CourseCount)> CountCoursesPerDepartment()
        {
            var result = _context.Courses
                .AsNoTracking()
                .GroupBy(crs => crs.Department.Name)
                .Select(DeptGroup => new
                {
                    DeptKey = DeptGroup.Key,
                    CourseCount = DeptGroup.Count()
                })
                .AsEnumerable()
                .Select(x => (x.DeptKey, x.CourseCount))
                .ToList();

            return result;
        }

        public void Delete(int id)
        {
            var deletedCourse = Get(id);
            _context.Departments.Remove(deletedCourse);
            _context.SaveChanges();
        }
    }
}
