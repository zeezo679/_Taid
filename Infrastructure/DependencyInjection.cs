using Core.Models.Interfaces.Messages;
using Core.Models.Interfaces.Trainee;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure;
using Web.Models.Entities;
using Web.Models.Interfaces;
using Web.Models.Repository;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("constr")));
        services.AddIdentity<ApplicationUser, IdentityRole>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
        services.AddControllersWithViews();  //infrastructure?
        services.AddSignalR();

        return services;    
    }
    
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IInstructorRepository, InstructorRepository>();
        services.AddScoped<ITraineeRepository, TraineeRepository>();
        services.AddScoped<ICrsResultRepository, CrsResultRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        
        return services;
    }
}