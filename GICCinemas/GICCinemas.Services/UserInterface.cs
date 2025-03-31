namespace GICCinemas.Services
{
    public class UserInterface: IUserInterface
    {
        private readonly IBookingService bookingService;

        public UserInterface(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        bool isDataInitiated { get; set; } //A boolean flag to check if data is already initiated
        public void InitData()
        {
            bookingService.InitData();
            isDataInitiated = true;
        }

        class Menu
        {
            public string Label { get; set; } //Label of the menu
            public int Index { get; set; } //Index of the menu
            public Func<bool>? Executer { get; set; } //The method to be executed if menu is selected

            public Menu(string label, Func<bool>? executer)
            {
                this.Label = label;
                this.Executer = executer;
            }
        }

        // Return true if user selected Exit
        public bool DisplayMenu()
        {
            // Register menu labels and executers
            var commands = new List<Menu>()
            {
                new Menu($"Book tickets for {bookingService.GetMovieTitle()} ({bookingService.GetAvailableSeatCount()} seats available)", BookTicket),
                new Menu("Check bookings", CheckBooking),
                new Menu("Exit", null)
            };
            Console.WriteLine();
            Console.WriteLine("Welcome to GIC Cinemas");
            int index = 1;
            foreach (var menu in commands) //Write out all commands for main menu
            {
                menu.Index = index++;
                Console.WriteLine($"[{menu.Index}] {menu.Label}");
            }
            Console.WriteLine("Please enter your selection:");

            // Keep prompting for input until it's a valid menu number
            int input = 0;
            while (!Int32.TryParse(Console.ReadLine(), out input)
                || !commands.Any(c => c.Index == input))
            {
                Console.WriteLine("Please enter a valid menu number:");
            }

            // Execute the registered method according to menu number
            var command = commands.Find(c => c.Index == input);
            if (command != null && command.Executer != null)
            {
                command.Executer();
                return false;
            }

            // If there's no registered executer, exit program
            Console.WriteLine();
            Console.WriteLine("Thank you for using GIC Cinemas system. Bye!");
            return true;
        }

        // Initiate book ticket flow
        bool BookTicket()
        {
            if (!isDataInitiated) InitData();
            bookingService.BookTicket();
            return true;
        }

        // Initiate check booking flow
        bool CheckBooking()
        {
            bookingService.CheckBooking();
            return true;
        }
    }
}