using CalendarBookingApp;
using CalendarBookingApp.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    private static ServiceProvider CreateServices()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IBookingManager, BookingManager>()
            .AddScoped<IBookingRepository, BookingRepository>()
            .AddDbContext<BookingContext>(ops =>
            {
                ops.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Booking;Trusted_Connection=True;");
            }, contextLifetime: ServiceLifetime.Scoped)
            .BuildServiceProvider();

        return serviceProvider;
    }

    public static void Main(string[] args)
    {
        var services = CreateServices();

        IBookingManager bookingManager = services.GetRequiredService<IBookingManager>();

        Console.WriteLine(Constants.title);
        Console.WriteLine(Constants.instruction);

        while (true)
        {
            string command = Console.ReadLine();

            if (command != null)
            {
                var request = ReadRequest(command);

                if (request.Action != null)
                {
                    switch (request.Action)
                    {
                        case "ADD":
                            bookingManager.AddAppointment(request);
                            break;
                        case "DELETE":
                            bookingManager.DeleteAppointment(request);
                            break;
                        case "FIND":
                            bookingManager.FindAppointment(request);
                            break;
                        case "KEEP":
                            bookingManager.KeepAnAppointment(request);
                            break;
                        default:
                            Console.WriteLine("Invalid Command");
                            break;
                    }
                }
                else
                    Console.WriteLine("Invalid Command");
            }
        }
    }

    private static CommandRequest ReadRequest(string command)
    {
        string[] commands = command.Split(' ');
        CommandRequest request = new CommandRequest();

        if (commands.Length > 0) {

            request.Action = commands[0].ToUpper();

            if (IsValidCommand(request.Action))
            {
                if (commands.Length == 3)
                {
                    request.DateMonth = commands[1];
                    request.TimeSlot = commands[2];
                }
                else if (commands.Length == 2)
                {
                    if (request.Action.Equals("FIND"))
                        request.DateMonth = commands[1];

                    if (request.Action.Equals("KEEP"))
                        request.TimeSlot = commands[1];
                }
            }
        }

        return request;
    }

    public static bool IsValidCommand(string commandAction)
    {
        var supportedCommands = new[] { "ADD", "DELETE", "FIND", "KEEP" };

        return supportedCommands.Contains(commandAction);
    }
}