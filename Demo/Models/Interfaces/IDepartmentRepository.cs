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
        void Delete(int id);
    }
}
