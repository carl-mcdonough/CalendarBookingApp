using System;

namespace CalendarBookingApp
{
    public class BookingManager : IBookingManager
    {
        private IBookingRepository _bookingRepository { get; set; }
        public BookingManager(IBookingRepository bookingRepository) 
        {
            _bookingRepository = bookingRepository;
        }

        public void AddAppointment(CommandRequest commandRequest)
        {
            try
            {
                DateTime date;
                if (DateTime.TryParseExact(commandRequest.DateMonth, "dd/MM", null, System.Globalization.DateTimeStyles.None, out date))
                {
                    var requestedAppointment = date.Date.Add(TimeSpan.Parse(commandRequest.TimeSlot));

                    if (IsFutureDate(requestedAppointment) && IsValidTimeslot(requestedAppointment) && !IsReservedTimeslot(requestedAppointment))
                    {
                        var currentBookings = _bookingRepository.RetrieveBookings()
                                          .Where(x => x.AppointmentSchedule.Equals(requestedAppointment))
                                          .Select(y => y.AppointmentSchedule)
                                          .ToList();

                        if (!currentBookings.Contains(requestedAppointment))
                        {
                            var newAppointment = new Bookings()
                            {
                                AppointmentSchedule = requestedAppointment
                            };

                            if (_bookingRepository.SaveAnAppointment(newAppointment) > 0)
                            {
                                Console.WriteLine("Appointment successfully added");
                                return;
                            }
                            else
                            {
                                Console.WriteLine("Appointment not added. Please try again and enter a valid command");
                                return;
                            }
                        }

                        Console.WriteLine("Timeslot already taken");
                        return;
                    }

                    Console.WriteLine("Appointment Date and Time not accepted. Please try another schedule");
                    return;
                }

                Console.WriteLine("Invalid Date Format. Please refer to the Command guide");
            }
            catch (Exception ex) 
            {
                Console.WriteLine("ADD Appointment failed. Please try again and enter a valid command");
            }
        }

        public void DeleteAppointment(CommandRequest commandRequest)
        {
            DateTime date;
            if (DateTime.TryParseExact(commandRequest.DateMonth, "dd/MM", null, System.Globalization.DateTimeStyles.None, out date))
            {
                var requestedAppointment = date.Date.Add(TimeSpan.Parse(commandRequest.TimeSlot));

                var currentBookings = _bookingRepository.RetrieveBookings()
                                          .Where(x => x.AppointmentSchedule.Equals(requestedAppointment))
                                          .Select(y => y.AppointmentSchedule)
                                          .ToList();

                if (currentBookings.Count == 0)
                {
                    Console.WriteLine("Appointment not found");
                    return;
                }

                if (_bookingRepository.DeleteAnAppointment(requestedAppointment))
                {
                    Console.WriteLine("Appointment successfully deleted");
                    return;
                }
                else
                {
                    Console.WriteLine("Appointment not deleted");
                    return;
                }
            }

            Console.WriteLine("Invalid Date Format. Please refer to the Command guide");
        }

        public void FindAppointment(CommandRequest commandRequest)
        {
            DateTime date;
            if (DateTime.TryParseExact(commandRequest.DateMonth, "dd/MM", null, System.Globalization.DateTimeStyles.None, out date))
            {
                var requestedDate = date.Date;

                if (IsFutureDate(requestedDate))
                {
                    List<string> availableTimeslots = new List<string>();
                    var currentBookings = _bookingRepository.RetrieveBookings()
                                          .Where(x => x.AppointmentSchedule.Date.Equals(requestedDate))
                                          .Select(y => y.AppointmentSchedule)
                                          .ToList();

                    for (int hour = 9; hour <= 17; hour++)
                    {
                        for (int minute = 0; minute < 60; minute += 30)
                        {
                            string time = $"{hour:00}:{minute:00}";
                            DateTime timeslotDateTime = date.Date.Add(TimeSpan.Parse(time));
                            if (IsValidTimeslot(timeslotDateTime) && !IsReservedTimeslot(timeslotDateTime))
                            {
                                if (!currentBookings.Contains(timeslotDateTime))
                                    availableTimeslots.Add(time);
                            }
                        }
                    }

                    if (availableTimeslots.Count > 0)
                    {
                        Console.WriteLine("Available timeslots:");
                        foreach (string timeslot in availableTimeslots)
                        {
                            Console.WriteLine(timeslot);
                        }
                        return;
                    }
                }
                Console.WriteLine("Invalid Date. Please enter a valid date");
                return;
            }

            Console.WriteLine("Invalid Date Format. Please refer to the Command guide");
        }

        public void KeepAnAppointment(CommandRequest commandRequest)
        {
            var keepDateTime = DateTime.Today.Date.Add(TimeSpan.Parse(commandRequest.TimeSlot));

            if (IsValidTimeslot(keepDateTime))
            {
                var currentBookings = _bookingRepository.RetrieveBookings()
                                          .Where(x => x.AppointmentSchedule.Equals(keepDateTime))
                                          .Select(y => y.AppointmentSchedule)
                                          .ToList();

                if (!currentBookings.Contains(keepDateTime))
                {
                    var newAppointment = new Bookings()
                    {
                        AppointmentSchedule = keepDateTime
                    };

                    if (_bookingRepository.SaveAnAppointment(newAppointment) > 0)
                    {
                        Console.WriteLine("Timeslot successfully kept");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Unable to keep the Timeslot");
                        return;
                    }
                }

                Console.WriteLine("Timeslot already taken");
                return;
            }

            Console.WriteLine("Invalid Time Slot");
        }

        #region Validations
        
        public static bool IsFutureDate(DateTime dateTime)
            => dateTime >= DateTime.Today.Date;

        public static bool IsValidTimeslot(DateTime dateTime)
        => ((dateTime.Hour >= 9 && dateTime.Hour < 17 && dateTime.Minute % 30 == 0) || dateTime.Hour == 17 && dateTime.Minute == 0);

        public static bool IsReservedTimeslot(DateTime timeSlotBeingBooked)
        {
            // Get the first day of the month of the requested time slot
            DateTime firstDayOfMonth = new DateTime(timeSlotBeingBooked.Year, timeSlotBeingBooked.Month, 1);
            DateTime startDateOfThirdWeek = firstDayOfMonth.AddDays(15 - (int)firstDayOfMonth.DayOfWeek);
            DateTime secondDayOfThirdWeek = startDateOfThirdWeek.AddDays(1);

            if (timeSlotBeingBooked.Date.Equals(secondDayOfThirdWeek.Date) &&
                (timeSlotBeingBooked.Hour >= 16 && timeSlotBeingBooked.Hour <= 17))
                return true;

            return false;
        }

        #endregion
    }
}
