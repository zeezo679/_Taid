using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web.Infrastructure;
using Web.Models.Entities;
using Web.Models.Interfaces;
using Web.ViewModel;

namespace Web.Models.Repository
{
    public class InstructorRepository : IInstructorRepository
    {
        private AppDbContext _context;

        public InstructorRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<Instructor> Load()
        {
            var instructors = _context.Instructors.Include(i => i.Course).Include(i => i.Department).ToList();
            return instructors;
        }

        public List<SelectListItem> LoadSelectItems()
        {
            var instructors = _context.Instructors.Include(i => i.Course).Include(i => i.Department).Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Name
            }).ToList();

            return instructors;
        }

        public List<Instructor> LoadInstructorsWithTheirCourses(CourseViewModel CourseView, bool save)
        {
           var instructors = _context.Instructors
                .Include(i => i.Course)
                .Include(i => i.Department)
                .Where(i => CourseView.InstructorIds.Contains(i.Id)).ToList();

            if(save) _context.SaveChanges();
            return instructors;
        }

        public Instructor Get(int id)
        {
            Instructor instructor = _context.Instructors.Include(i => i.Course).Include(i => i.Department).FirstOrDefault(c => c.Id == id);
            return instructor;
        }
        public void Insert(Instructor instructor)
        {
            _context.Instructors.Add(instructor);
            _context.SaveChanges();
        }
        public void Update(int id, Instructor newInstructor)
        {
            var oldInstructor = Get(id);
            oldInstructor.Name = newInstructor.Name;
            oldInstructor.Salary = newInstructor.Salary;
            oldInstructor.Address = newInstructor.Address;
            oldInstructor.Image = newInstructor.Image;
            oldInstructor.Department = newInstructor.Department;
            Console.WriteLine(oldInstructor.Department.Name);

            _context.SaveChanges();
        }

        public void Update(Instructor oldInstructor, Instructor newInstructor)
        {
            oldInstructor.Name = newInstructor.Name;
            oldInstructor.Salary = newInstructor.Salary;
            oldInstructor.Address = newInstructor.Address;
            oldInstructor.Image = newInstructor.Image;
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var deletedInstructor = Get(id);
            _context.Instructors.Remove(deletedInstructor);
            _context.SaveChanges();
        }

        public async Task<List<Course>> FilterCoursesByCurrentInstructorAsync(string uid)
        {

            var instructor = await _context.Instructors
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(i => i.UserId == uid);

            var courseId = instructor.CourseId; //storing in memory to avoid exception

            var courses = await _context.Courses
                .AsNoTracking()
                .Include(c => c.Department)
                .Where(c => c.Id == courseId)
                .ToListAsync();
           

            return courses;
        }
    }
}
