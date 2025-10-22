using Demo.Infrastructure;
using Demo.Models.Entities;
using Demo.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Demo.Models.Repository
{
    public class TraineeRepository : ITraineeRepository
    {
        private AppDbContext _context;
        private ICourseRepository _courseRepository;
        private ICrsResultRepository _crsResultRepository;

        public TraineeRepository(AppDbContext context, ICourseRepository courseRepository, ICrsResultRepository crsResultRepository)
        {
            _context = context;
            _courseRepository = courseRepository;
            _crsResultRepository = crsResultRepository;
        }
        public List<Trainee> Load(bool ordered)
        {
            List<Trainee> trainees = ordered ?
                _context.Trainees.Include(t => t.Department).OrderByDescending(t => t.Grade).ToList() :
                _context.Trainees.Include(t => t.Department).ToList();

            return trainees;
        }

        public Trainee Get(int id)
        {
            var trainee = _context.Trainees.Include(t => t.Department).FirstOrDefault(t => t.Id == id);
            if (trainee == null)
                throw new KeyNotFoundException($"Maybe the trainee is not in the database: id {id}");
            
            return trainee;
        }

        public bool IsAlreadyTrainee(string UID)
        {
            var trainee = _context.Trainees.FirstOrDefault(t => t.UserId == UID);
            if(trainee != null)
                return true;
            return false;
        }

        public void Insert(Trainee trainee)
        {
            _context.Trainees.Add(trainee);
            _context.SaveChanges();
        }

        public void InsertTraineeCrsResult(Trainee trainee, int crsId)
        {
            _context.Trainees.Add(trainee);
            _context.SaveChanges();
        }


        public void Update(int id, Trainee trainee)
        {
            if(trainee != null)
            {
                Trainee oldTrainee = Get(id);
                oldTrainee.Name = trainee.Name;
                oldTrainee.Address = trainee.Address;
                oldTrainee.Image = trainee.Image;
                oldTrainee.Grade = trainee.Grade;
                oldTrainee.Department = trainee.Department;


                _context.SaveChanges();
            }
        }



        public async Task DeleteAsync(int id)
        {
            var trainee = Get(id);
             _context.Remove(trainee);
             await _context.SaveChangesAsync();
        }


    }
}
