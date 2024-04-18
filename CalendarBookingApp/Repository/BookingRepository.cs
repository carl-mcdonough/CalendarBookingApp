using Microsoft.EntityFrameworkCore;

namespace CalendarBookingApp.Repository
{
    public class BookingRepository : IBookingRepository
    {
        readonly BookingContext _bookingContext;

        public BookingRepository(BookingContext context) 
        {
            _bookingContext = context;
        }

        public int SaveAnAppointment(Bookings appointment)
        {
            _bookingContext.Add(appointment);

            if (_bookingContext.SaveChanges() > 0)
                return appointment.Id;

            return 0;
        }

        public bool DeleteAnAppointment(DateTime appointmentToRemove)
        {
            return _bookingContext.Bookings.Where(x => x.AppointmentSchedule.Equals(appointmentToRemove)).ExecuteDelete() > 0 ? true : false;
        }

        public DbSet<Bookings> RetrieveBookings()
        {
            return _bookingContext.Bookings;
        }

        public bool UpdateATimeSlot()
        {
            if (_bookingContext.SaveChanges() > 0)
                return true;

            return false;
        }
    }
}
