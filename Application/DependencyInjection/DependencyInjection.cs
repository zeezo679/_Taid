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
using Web.Models;
using Web.Models.Entities;
using Web.Models.Interfaces;

namespace Web.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailOptions>(configuration.GetSection("EmailSettings"));
            services.AddTransient<EmailService>();
            services.AddScoped<IMapper, Mapper>();
            services.AddScoped<ITraineeService, TraineeService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddSingleton<IMessageQueue, MessageQueue>();
            services.AddHostedService<MessageSaverService>();

            return services;
        }
  
    }
}
