using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CalendarBookingApp
{
    public class BookingContext : DbContext
    {
        public DbSet<Bookings> Bookings { get; set; } = null!;
        IConfiguration appConfig;

        public BookingContext() { }

        public BookingContext(IConfiguration config)
        {
            appConfig = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Bookings;Trusted_Connection=True;");
            //optionsBuilder.UseSqlServer(appConfig.GetConnectionString("AzureSQL"));
        }
    }
}
