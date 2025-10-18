using Demo.Infrastructure;
using Demo.Models.Entities;
using Demo.Models.Interfaces;

namespace Demo.Models.Repository
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
                UserId = trainee.UserId
            };

            _context.crsResults.Add(crsResult);

            _context.SaveChanges();
        }

        public void IsEnrolled(string UID)
        {
           
        }
    }
}
