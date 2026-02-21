using Microsoft.EntityFrameworkCore;
using trackandrive.dashcam.ViewModel;

namespace TrackanDrive.Dashcam.Models
{
    public partial class TrackanDriveDbContext : DbContext
    {
        public TrackanDriveDbContext(DbContextOptions<TrackanDriveDbContext> options)
            : base(options)
        {
        }
        public DbSet<Pushing_Dashcam_History_Data> Pushing_Dashcam_History_Data { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleModels> VehicleModels { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

    }
}
