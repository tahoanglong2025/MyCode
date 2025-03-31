using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GICCinemas.Services;

// Register services
var serviceProvider = new ServiceCollection()
    .AddTransient<IUserInterface, UserInterface>()
    .AddTransient<IBookingService, BookingService>()
    .AddTransient<ISeatRepository, SeatRepository>()
    .BuildServiceProvider();

// Register logger
using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole().AddDebug());
ILogger logger = factory.CreateLogger("GICCinemas");

// Get UserInterface service to start the UI
var userInterface = serviceProvider.GetService<IUserInterface>();

if (userInterface != null)
{
    try
    {
        userInterface.InitData(); //Initiate seats data input first thing before allowing booking
        bool exit;
        do
        {
            exit = userInterface.DisplayMenu(); //Keep showing main menu until user selects Exit
        } while (!exit);
    } catch (Exception ex)
    {
        Console.WriteLine("Opps! We experienced an error. Please contact GIC. Appologize for the inconvenience.");
        logger.LogInformation(ex.ToString());
        logger.LogDebug(ex, "Exception");
    }
}

// write a function to
// find all images without alternate text
// and give them a red border
