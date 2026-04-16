using DashboardData.Models;
namespace DashboardData.Services
{
    public class SensorService : ISensorService
    {
        private readonly AppDbContext _context;

    // L'Injection de dépendance fait le travail ici :
    // Quand Blazor crée le SensorService, il lui passe automatiquement le DbContext.
    public SensorService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<SensorData>> GetSensorsAsync()
{
    // EF Core traduit Include par un JOIN SQL vers la table Location
    return await _context.Sensors
        .Include(s => s.Location) 
        .ToListAsync();
}
        private readonly List<SensorData> _sensors = new List<SensorData>()
        {
            new SensorData { Name = "Temp_Salon", Value = 22.5 },
            new SensorData { Name = "Hum_Cuisine", Value = 45.0 },
            new SensorData { Name = "C02_Bureau", Value = 800 },
            new SensorData { Name = "Temp_Chambre", Value = 20.0 },
            new SensorData { Name = "Hum_Salon", Value = 40.0 },
            new SensorData { Name = "Temp_Bureau", Value = 600 },
        };
        public async Task<List<SensorData>> GetSensorsAsync()
{
    // EF Core traduit Include par un JOIN SQL vers la table Location
    return await _context.Sensors
        .Include(s => s.Location) 
        .ToListAsync();
}
        public async Task AddSensorAsync(SensorData sensor)
{
    // 1. On prépare l'ajout en mémoire
    _context.Sensors.Add(sensor);
    
    // 2. On valide la transaction (Génère le INSERT INTO SQL)
    await _context.SaveChangesAsync(); 
}
public async Task<List<SensorData>> GetCriticalSensorsAsync(double threshold)
{
    return await _context.Sensors
        .Include(s => s.Location)
        .Where(s => s.Value > threshold) // Traduit en : WHERE Value > @threshold
        .OrderByDescending(s => s.Value) // Traduit en : ORDER BY Value DESC
        .ToListAsync();                  // Déclenche l'exécution SQL
}
public async Task<double> GetAverageValueAsync()
        {
            if (!await _dbContext.Sensors.AnyAsync()) return 0;

            return await _dbContext.Sensors.AverageAsync(s => s.Value);
        }

        public async Task<double> GetMaxValueAsync()
        {
            if (!await _dbContext.Sensors.AnyAsync()) return 0;
            return await _dbContext.Sensors.MaxAsync(s => s.Value);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _dbContext.Sensors.CountAsync();
        }
    }
    public async Task<List<Location>> GetLocationsAsync()
{
    return await _context.Locations.ToListAsync();
}

public async Task<SensorData?> GetSensorByIdAsync(int id)
{
    // FindAsync cherche directement par la Clé Primaire (Id)
    return await _context.Sensors.FindAsync(id);
}

public async Task AddSensorAsync(SensorData sensor)
{
    sensor.LastUpdate = DateTime.Now;
    
    // Historisation de la valeur initiale (TP5)
    sensor.Values.Add(new SensorValueHistory {
        MeasuredValue = sensor.Value,
        Date = DateTime.Now
    });

    _context.Sensors.Add(sensor);
    await _context.SaveChangesAsync();
}

public async Task UpdateSensorAsync(SensorData sensor)
{
    sensor.LastUpdate = DateTime.Now; // Mise à jour de la date
    
    // Ajout à l'historique lors d'une modification (TP5)
    sensor.Values.Add(new SensorValueHistory {
        MeasuredValue = sensor.Value,
        Date = DateTime.Now
    });

    _context.Sensors.Update(sensor);
    await _context.SaveChangesAsync();
}

public async Task DeleteSensorAsync(int id)
{
    var sensor = await _context.Sensors.FindAsync(id);
    if (sensor != null)
    {
        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync();
    }
}
}
