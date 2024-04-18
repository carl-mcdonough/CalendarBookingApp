namespace CalendarBookingApp
{
    public interface IBookingManager
    {
        void AddAppointment(CommandRequest commandRequest);
        void DeleteAppointment(CommandRequest commandRequest);
        void FindAppointment(CommandRequest commandRequest);
        void KeepAnAppointment(CommandRequest commandRequest);
    }
}