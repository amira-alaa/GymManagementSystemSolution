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
        public static bool SeedData(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager)
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
                        if (!roleManager.RoleExistsAsync(role.Name!).Result)
                        {
                            roleManager.CreateAsync(role).Wait();
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
                    userManager.CreateAsync(SuperAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(SuperAdmin, "SuperAdmin").Wait();

                    var Admin = new ApplicationUser()
                    {

                        FirstName = "Ahmed",
                        LastName = "Alaa",
                        UserName = "AhmedAlaa",
                        Email = "AhmedAlaa@gmail.com",
                        PhoneNumber = "01029881438"
                    };
                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();


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
