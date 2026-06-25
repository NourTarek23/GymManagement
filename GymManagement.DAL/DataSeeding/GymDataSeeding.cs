using GymManagement.DAL.Models;
using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagement.DAL.DataSeeding;

public class GymDataSeeding
{
    public static async Task SeedAsync(GymDbContext context, string seedFolderPath, ILogger logger, CancellationToken ct = default)
    {
        try
        {
            if (!context.Plans.Any())
            {
                var plans =  LoadDataFromJsonFile<Plan>(seedFolderPath, "plans.json");

                //Add to dataBase
                await context.Plans.AddRangeAsync(plans);
                await context.SaveChangesAsync(ct);
                logger.LogInformation($"Seeding Plans with count {plans.Count}");
            }

        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }

    public static List<T> LoadDataFromJsonFile<T>(string folderPath, string fileName) where T : BaseEntity
    {
        var filePath = Path.Combine(folderPath, fileName);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"seed data file not found: {filePath}");

        //Read data from json file
        var data = File.ReadAllText(filePath);
        var option = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };

        //convert jsonString to List<plan>
        var result = JsonSerializer.Deserialize<List<T>>(data, option) ?? [];

        return result;
    }
}
