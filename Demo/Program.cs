using Demo.Infrastructure;
using Demo.Models.Entities;
using Demo.Models.Interfaces;
using Demo.Models.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using Demo.DependencyInjection;
using Demo.Hubs;
using Demo.Models;
using Demo.Services;

//Continue RoleInitialization Class

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables();


builder.Services.Register(builder);
builder.Services.AddSingleton<IMessageQueue, MessageQueue>();
builder.Services.AddHostedService<MessageSaverService>();
builder.Services.AddSignalR();
builder.Services.AddAuthentication()
    .AddGoogle(o =>
    {
        o.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        o.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    await InitializeRolesAsync(roleManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

      
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    ).WithStaticAssets();

app.MapHub<AdminChatHub>("/chatHub");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Course}/{action=Index}"
//).WithStaticAssets();


app.Run();


//Helper Methods
static async Task InitializeRolesAsync(RoleManager<IdentityRole> roleManager)
{
    string[] roles = { "Admin", "Trainee", "Instructor", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}