using DashboardData.Models;
namespace DashboardData.Services
{
    public interface ISensorService
    {
        Task<List<SensorData>> GetSensorDataAsync();
        Task<List<SensorData>> GetCriticalSensorsAsync(double threshold);
        Task<int> GetTotalCountAsync();
        Task<double> GetAverageValueAsync();
        Task<double> GetMaxValueAsync();
        void AddSensor(SensorData sensorData);
    }
}
