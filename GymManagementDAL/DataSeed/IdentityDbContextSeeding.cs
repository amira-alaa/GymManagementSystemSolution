using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManagementDAL.DataSeed
{
    public class IdentityDbContextSeeding
    {
        public static async Task<bool> SeedDataAsync(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            try
            {
                var IsUsersExist = userManager.Users.Any();
                var IsRolesExist = roleManager.Roles.Any();
                if (IsUsersExist && IsRolesExist) return false;
                if (!IsRolesExist)
                {
                    var Roles = new List<IdentityRole>()
                {
                    new(){ Name = "Admin"},
                    new(){ Name = "SuperAdmin"}
                };

                    foreach (var role in Roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role.Name!))
                        {
                            await roleManager.CreateAsync(role);
                        }
                    }
                }

                if (!IsUsersExist)
                {
                    var SuperAdmin = new ApplicationUser()
                    {

                        FirstName = "Amira",
                        LastName = "Alaa",
                        UserName = "AmiraAlaa",
                        Email = "alaaamira898@gmail.com",
                        PhoneNumber = "01276991038"
                    };
                    await userManager.CreateAsync(SuperAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(SuperAdmin, "SuperAdmin");

                    var Admin = new ApplicationUser()
                    {

                        FirstName = "Ahmed",
                        LastName = "Alaa",
                        UserName = "AhmedAlaa",
                        Email = "AhmedAlaa@gmail.com",
                        PhoneNumber = "01029881438"
                    };
                    await userManager.CreateAsync(Admin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(Admin, "Admin");


                }

                return true;
            }catch(Exception ex)
            {
                Console.WriteLine($"Failed To Seed : {ex}");
                return false;
            }
        }
    }
}
