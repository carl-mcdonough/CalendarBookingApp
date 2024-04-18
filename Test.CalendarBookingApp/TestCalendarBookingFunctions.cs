using CalendarBookingApp;

namespace Test.CalendarBookingApp
{
    public class TestCalendarBookingFunctions
    {

        [TestCase("ADD")]
        [TestCase("DELETE")]
        [TestCase("FIND")]
        [TestCase("KEEP")]

        public void CommandShouldBeSupported(string commandKeyword)
        {
            Assert.That(Program.IsValidCommand(commandKeyword), Is.EqualTo(true));
        }

        [TestCase("SAVE")]
        [TestCase("REMOVE")]
        [TestCase("SELECT")]
        [TestCase("UPDATE")]

        public void DoNotAcceptUnsupportedCommand(string commandKeyword)
        {
            Assert.That(Program.IsValidCommand(commandKeyword), Is.EqualTo(false));
        }

        [TestCase("19/04/2024 3:30 PM")]
        [TestCase("19/05/2024 10:30 AM")]
        [TestCase("23/07/2024 9:00 AM")]
        public void TimeSlotShouldBeEqualTo30Minutes(string dateTime)
        {
            DateTime dateTimeToValidate = Convert.ToDateTime(dateTime);
            Assert.That(BookingManager.IsValidTimeslot(dateTimeToValidate), Is.EqualTo(true));
        }

        [TestCase("19/04/2024 3:35 PM")]
        [TestCase("19/05/2024 10:38 AM")]
        [TestCase("23/07/2024 9:02 AM")]
        public void DoNotAcceptTimeSlotNotEqualTo30Minutes(string dateTime)
        {
            DateTime dateTimeToValidate = Convert.ToDateTime(dateTime);
            Assert.That(BookingManager.IsValidTimeslot(dateTimeToValidate), Is.EqualTo(false));
        }

        [TestCase("19/04/2024 3:30 PM")]
        [TestCase("19/05/2024 10:30 AM")]
        [TestCase("23/07/2024 9:00 AM")]
        public void TimeSlotShouldBeBetweenAcceptableTime(string dateTime)
        {
            DateTime dateTimeToValidate = Convert.ToDateTime(dateTime);
            Assert.That(BookingManager.IsValidTimeslot(dateTimeToValidate), Is.EqualTo(true));
        }

        [TestCase("12/04/2024 5:30 PM")]
        [TestCase("19/05/2024 6:00 PM")]
        [TestCase("23/07/2024 8:30 AM")]
        public void DoNotAcceptTimeSlotOutsideAcceptableTime(string dateTime)
        {
            DateTime dateTimeToValidate = Convert.ToDateTime(dateTime);
            Assert.That(BookingManager.IsValidTimeslot(dateTimeToValidate), Is.EqualTo(false));
        }

        [TestCase("16/04/2024 3:00 PM")]
        [TestCase("14/05/2024 3:30 PM")]
        [TestCase("16/07/2024 2:00 PM")]
        public void TimeSlotShouldNotBeOnReservedSlot(string dateTime)
        {
            DateTime dateTimeToValidate = Convert.ToDateTime(dateTime);
            Assert.That(BookingManager.IsReservedTimeslot(dateTimeToValidate), Is.EqualTo(false));
        }

        [TestCase("16/04/2024 5:00 PM")]
        [TestCase("14/05/2024 4:30 PM")]
        [TestCase("16/07/2024 4:00 PM")]
        public void DoNotAcceptTimeSlotOnReservedSlot(string dateTime)
        {
            DateTime dateTimeToValidate = Convert.ToDateTime(dateTime);
            Assert.That(BookingManager.IsReservedTimeslot(dateTimeToValidate), Is.EqualTo(true));
        }
    }
}