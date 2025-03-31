using GICCinemas.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GICCinemas.Test
{
    public class Startup
    {
        public Startup()
        {
            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTransient<IUserInterface, UserInterface>()
                .AddTransient<IBookingService, BookingService>()
                .AddTransient<ISeatRepository, SeatRepository>()
                .BuildServiceProvider();
        }
    }
}
