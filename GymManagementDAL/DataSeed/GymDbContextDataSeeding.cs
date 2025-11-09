using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;

namespace GymManagementDAL.DataSeed
{
    public class GymDbContextDataSeeding
    {
        public static async Task<bool> DataSeedAsync(GymDbContext dbContext)
        {
            try
            {
                var IsPlansExist = dbContext.Plans.Any();
                var IsCategoriesExist = dbContext.Categories.Any();
                if (IsPlansExist && IsCategoriesExist) return false;
                if (!IsPlansExist)
                {
                    var Plans = await LoadDataAsync<Plan>("Plans.json");
                    if (Plans.Any()) dbContext.Plans.AddRange(Plans);
                }
                if (!IsCategoriesExist)
                {
                    var Categories = await LoadDataAsync<Category>("Categories.json");
                    if (Categories.Any()) await dbContext.Categories.AddRangeAsync(Categories);
                }
                return dbContext.SaveChanges() > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Seeding Failed : {ex}");
                return false;
            }
        }
        private static async Task<List<T>> LoadDataAsync<T>(string filePath)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", filePath);
            if (!File.Exists(FilePath)) throw new FileNotFoundException();
            //string Data = await File.ReadAllTextAsync(FilePath);
            using var Data = File.OpenRead(FilePath);
            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            return await JsonSerializer.DeserializeAsync<List<T>>(Data, Options) ?? new List<T>();
        }
    }
}
