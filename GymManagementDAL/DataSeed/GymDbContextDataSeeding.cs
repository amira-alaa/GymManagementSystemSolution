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
        public static bool DataSeed(GymDbContext dbContext)
        {
            try
            {
                var IsPlansExist = dbContext.Plans.Any();
                var IsCategoriesExist = dbContext.Categories.Any();
                if (IsPlansExist && IsCategoriesExist) return false;
                if (!IsPlansExist)
                {
                    var Plans = LoadData<Plan>("Plans.json");
                    if (Plans.Any()) dbContext.Plans.AddRange(Plans);
                }
                if (!IsCategoriesExist)
                {
                    var Categories = LoadData<Category>("Categories.json");
                    if (Categories.Any()) dbContext.Categories.AddRange(Categories);
                }
                return dbContext.SaveChanges() > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Seeding Failed : {ex}");
                return false;
            }
        }
        private static List<T> LoadData<T>(string filePath)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", filePath);
            if (!File.Exists(FilePath)) throw new FileNotFoundException();
            string Data = File.ReadAllText(FilePath);
            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();
        }
    }
}
