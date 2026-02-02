using Core.Models.Interfaces;
using Core.Models.Interfaces.Messages;
using Core.Models.Interfaces.Trainee;
using Core.Models.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Web.Models.Entities;
using Web.Models.Interfaces;

namespace Web.DependencyInjection
{
    public static class ServicesExtension
    {
        public static void Register(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();

            RegisterRepositories(builder);

            builder.Services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("constr")));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddTransient<EmailService>();
            builder.Services.AddScoped<IMapper, Mapper>();
            builder.Services.AddScoped<ITraineeService, TraineeService>();
        }

        private static void RegisterRepositories(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
            builder.Services.AddScoped<ITraineeRepository, TraineeRepository>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ICrsResultRepository, CrsResultRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        }


    }
}
