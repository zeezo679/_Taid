using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Web.Infrastructure;
using Web.Models;
using Web.Models.Entities;
using Web.Models.Interfaces;
using Web.Models.Repository;

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
