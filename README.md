# CalendarBookingApp

**How to use**

Clone repository then run the project. You may need to update the connection string depending on how your localDB is setup.

In the command line, enter any of the following commands
- `ADD DD/MM hh:mm` to add an appointment
- `DELETE DD/MM hh:mm` to remove an appointment
- `FIND DD/MM` to find a free timeslot for the day
- `KEEP hh:mm` to keep a timeslot for any day

Constraints

- The time slot will be always equal to 30 minutes
- The acceptable time is between 9AM and 5PM
- 4PM to 5PM on each second day of the third week of any month is reverved and unavailable timeslot



**Areas to improve**

- Add more validations and error handling
- Refactor classes to simplify and lessen lines of code
- Add more unit tests
- Bring the connection string in a config file
- Add logging
- Create a user interface to make the app more interactive


**Technologies Used**
- C# Console App
- .Net 6.0
- Entity Framework 7.0.1
- SQL Server Express LocalDB
