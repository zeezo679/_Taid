using Microsoft.EntityFrameworkCore;
using Web.Infrastructure;
using Web.Models.Entities;
using Web.Models.Interfaces;
using Web.ViewModel;

namespace Web.Models.Repository
{
    public class CrsResultRepository : ICrsResultRepository
    {
        private readonly AppDbContext _context;


        public CrsResultRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Insert(Trainee trainee, int crsId)
        {

            var crsResult = new CrsResult
            {
                Degree = trainee.Grade,
                CourseId = crsId,
                UserId = trainee.UserId,
                EnrollmentDate = DateTime.Now
            };

            _context.crsResults.Add(crsResult);

            _context.SaveChanges();
        }

        public List<CrsResult> Load()
        {
            var enrollments = _context.crsResults.ToList();
            return enrollments;
        }
        
        public List<EnrollmentViewModel> GetRecentEnrollments()
        {
            var topFiveEnrollments = _context.crsResults
                .OrderByDescending(c => c.CourseId)
                .Take(5)
                .ToList();

            var recentEnrolls = _context.crsResults.Select(c => new EnrollmentViewModel
            {
                Id = c.CourseId,
                Name = c.Course.Name,
                TraineeName = c.Trainee.Name,
                EnrollmentDate = c.EnrollmentDate,
                Department = c.Course.Department,
                Instructors = c.Course.Instructors,
            })
                .AsEnumerable()
                .ToList();
            
            return recentEnrolls;
        }

        public bool CheckEnrollStatus(int crsId, string uid)
        {
            if (string.IsNullOrEmpty(uid) || crsId == 0)
                throw new ArgumentNullException();
            
            var row = _context.crsResults
                .AsNoTracking()
                .Where(crsr => crsr.CourseId == crsId && crsr.UserId == uid);
            
            return row.Any();
        }

        public async Task<List<CrsResult>> FilterCoursesByCurrentUserAsync(string uid)
        {
            if(string.IsNullOrEmpty(uid))
                return new List<CrsResult>();

            var courses = await _context.crsResults
                .AsNoTracking()
                .Include(crsr => crsr.Course)
                .Where(crsr => crsr.UserId == uid)
                .ToListAsync();

            return courses;
        }

        public void IsEnrolled(string UID)
        {
           //is inroller?
        }
    }
}
