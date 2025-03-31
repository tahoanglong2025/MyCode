namespace GICCinemas.Services
{
    public class BookingService: IBookingService
    {
        private readonly ISeatRepository seatRepository;
        public BookingService(ISeatRepository seatRepository)
        {
            this.seatRepository = seatRepository;
        }

        string MovieTitle { get; set; } = string.Empty;

        public string GetMovieTitle()
        {
            return MovieTitle;
        }

        public void InitData()
        {
            Console.WriteLine("Please define movie title and seating map in [Title] [Row] [SeatsPerRow] format:");
            string? result;
            do
            {
                result = SaveInitData(Console.ReadLine()); //Parse and save the inputed init data from console
                if (result != null) Console.WriteLine(result); //Display validation message (if any)
            } while (result != null); //Continue until init data is saved
        }

        // Return null if inputed data is valid and saved, or the validation message if inputed data is invalid
        string? SaveInitData(string? inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return "Please input in correct format [Title] [Row] [SeatsPerRow]:";

            var inputs = inputString.Split(' ');
            if (inputs.Length != 3)
                return "Please input in correct format [Title] [Row] [SeatsPerRow]:";

            if (!int.TryParse(inputs[1], out int row) || row <= 0 || row > 26)
                return "Row must be a valid positive number <= 26";

            if (!int.TryParse(inputs[2], out int seatsPerRow) || seatsPerRow <= 0 || seatsPerRow > 50)
                return "SeatsPerRow must be a valid positive number <= 50";

            // After passing all validations, valid inputed data is saved
            this.MovieTitle = inputs[0];
            seatRepository.InitData(row, seatsPerRow);

            return null; //Indicate that inputed data is valid and saved
        }

        public int GetAvailableSeatCount()
        {
            return seatRepository.GetAvailableSeatCount();
        }

        public void BookTicket()
        {
            Booking? booking = null;
            while (booking == null) //If no booking is confirmed yet, keep prompting for user inputs
            {
                int numberOfTikets = 0;
                Console.WriteLine();
                Console.WriteLine("Enter number of tickets to book, or enter blank to go back to main menu:");
                string? input;
                input = Console.ReadLine();
                // Keep prompting for number of tickets
                while (!Int32.TryParse(input, out numberOfTikets) || numberOfTikets <= 0)
                {
                    if (string.IsNullOrWhiteSpace(input)) return; //Stop if user inputs blank
                    Console.WriteLine("Please enter a valid number:");
                    input = Console.ReadLine();
                }
                booking = seatRepository.CreateBooking(numberOfTikets); //Get default seat selection

                // Keep prompting user to accept current seat selection or enter another custom seat position
                while (booking != null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Successfully reserved {numberOfTikets} {MovieTitle} tickets.");
                    seatRepository.PrintBooking(booking);
                    Console.WriteLine();
                    Console.WriteLine("Enter blank to accept seat selection, or enter new seating position:");
                    var seatInput = Console.ReadLine();
                    var parsedSeatInput = seatRepository.ParseSeatInput(seatInput);
                    while (parsedSeatInput == null) //Keep prompting for a valid custom seat position
                    {
                        if (string.IsNullOrWhiteSpace(seatInput))
                        {
                            seatRepository.SaveBooking(booking); //Confirm and stop if user enters blank
                            Console.WriteLine($"Booking id: {booking.Id} confirmed.");
                            return;
                        }
                        Console.WriteLine("Please enter a valid seat:");
                        seatInput = Console.ReadLine();
                        parsedSeatInput = seatRepository.ParseSeatInput(seatInput);
                    }
                    // Get custom seat selection
                    booking = seatRepository.CreateBooking(numberOfTikets, parsedSeatInput.Item1, parsedSeatInput.Item2);
                }
            }
        }

        public void CheckBooking()
        {
            string? inputBookingId = null;
            do //Keep prompting for booking id
            {
                Console.WriteLine();
                Console.WriteLine("Enter booking id, or enter blank to go back to main menu:");
                inputBookingId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputBookingId)) return; //Finish if user enters blank
                
                Console.WriteLine();
                var booking = seatRepository.GetBooking(inputBookingId);
                if (booking == null) Console.WriteLine($"Cannot find booking id {inputBookingId}.");
                else seatRepository.PrintBooking(booking);
            } while (!string.IsNullOrWhiteSpace(inputBookingId));
        }
    }
}