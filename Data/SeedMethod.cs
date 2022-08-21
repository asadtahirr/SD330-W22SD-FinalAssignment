using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using stack_overload.Data;

namespace stack_overload.Models
{
    public class SeedMethod
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();


            if (!context.Roles.Any())
            {
                List<string> roles = new List<string>()
                {
                    "Administrator"
                };

                foreach (string role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (!context.Users.Any())
            {
                User admin = new User
                {
                    Email = "admin@stackoverload.ca",
                    NormalizedEmail = "admin@stackoverload.ca",
                    UserName = "admin@stackoverload.ca",
                    NormalizedUserName = "admin@stackoverload.ca",
                    EmailConfirmed = true
                };

                User generalUser1 = new User
                {
                    Email = "gu1@stackoverload.ca",
                    NormalizedEmail = "gu1@stackoverload.ca",
                    UserName = "gu1@stackoverload.ca",
                    NormalizedUserName = "gu1@stackoverload.ca",
                    EmailConfirmed = true
                };

                User generalUser2 = new User
                {
                    Email = "gu2@stackoverload.ca",
                    NormalizedEmail = "gu2@stackoverload.ca",
                    UserName = "gu2@stackoverload.ca",
                    NormalizedUserName = "gu2@stackoverload.ca",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Pass123#");

                await userManager.AddToRoleAsync(admin, "Administrator");

                await userManager.CreateAsync(generalUser1, "Pass123#");

                await userManager.CreateAsync(generalUser2, "Pass123#");
            }

            context.SaveChanges();
        }

    }
}
