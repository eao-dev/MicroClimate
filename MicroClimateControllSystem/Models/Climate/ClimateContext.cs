using System.Data.Entity;

namespace MicroClimateControllSystem.Models
{
    public class ClimateContext : DbContext
    {
        public ClimateContext() : base("ClimateContext") =>
        Database.SetInitializer<ClimateContext>(null);

        // Все сущности БД
        public DbSet<Auth> Auth{ get; set; }
        public DbSet<Sensor> Sensor { get; set; }
        public DbSet<SensorData> SensorsData { get; set; }

    }
}