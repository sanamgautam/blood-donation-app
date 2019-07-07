using BloodDonationApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodDonationApp.Data
{
    public class SeedDatabase
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                User user = new User {
                    UserName = "admin",
                    SecurityStamp=Guid.NewGuid().ToString(),
                    Email = "blooddonationapp@gmail.com"
                };

                var result = await userManager.CreateAsync(user, "Admin@135");
                if (result.Succeeded)
                {
                    List<string> roles = new List<string>() { "Setting", "Add Blood Bank" };

                    foreach (string role in roles)
                    {
                        if (await roleManager.FindByNameAsync(role) == null)
                        {
                            await roleManager.CreateAsync(new Role { Id = Guid.NewGuid(), Name = role });
                        }
                    }

                    await userManager.AddToRolesAsync(user, roles);
                }
            }
        }
    }
}
