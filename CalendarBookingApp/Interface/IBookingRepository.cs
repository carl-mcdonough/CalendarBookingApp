using Microsoft.EntityFrameworkCore;

namespace CalendarBookingApp
{
    public interface IBookingRepository
    {
        int SaveAnAppointment(Bookings appointment);
        bool DeleteAnAppointment(DateTime appointmentToRemove);
        DbSet<Bookings> RetrieveBookings();
        bool UpdateATimeSlot();
    }
}
