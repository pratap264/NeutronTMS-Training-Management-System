using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using NeutronTMS.Models;
using NeutronTMS.Enums;

namespace NeutronTMS.Data;
public static class ContextSeed
{
    public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Default User
        var defaultUser = new ApplicationUser 
        { 
            UserName = "superadmin@gmail.com", 
            Email = "superadmin@gmail.com",
            FirstName = "tom",
            LastName = "jerry",
            EmailConfirmed = true, 
            PhoneNumberConfirmed = true 
        };

        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if(user == null)
            {
                await userManager.CreateAsync(defaultUser, "Admin@123");
                await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Trainee.ToString());
                await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Trainer.ToString());
                await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
                await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString());
            }
                
        }
    }


    public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Roles
        await roleManager.CreateAsync(new IdentityRole(Enums.Roles.SuperAdmin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Trainer.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Trainee.ToString()));
    }
}