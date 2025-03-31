namespace GICCinemas.Services
{
    public class Booking
    {
        public string Id { get; set; } = string.Empty;
        public List<Tuple<int, int>> Seats { get; set; } = new List<Tuple<int, int>>();
    }

    public class SeatRepository: ISeatRepository
    {
        int Rows { get; set; } //Store number of rows in the room
        int SeatsPerRow { get; set; } //Store number of seats per row in the room

        bool[,] Seats = new bool[26,50]; //2-dimension matrix to store seats' status (true is booked, false is available)

        List<Booking> Bookings = new List<Booking>(); //Store confirmed bookings

        public void InitData(int row, int seatsPerRow)
        {
            this.Rows = row;
            this.SeatsPerRow = seatsPerRow;
        }

        // Reserve seats with default seat selection logic, or with custom seat position if selectedRow and selectedSeat are passed
        // Return a draft booking details (id, selected seats)
        public Booking? CreateBooking(int numberOfTickets, int? selectedRow = null, int? selectedSeat = null)
        {
            int remainingSeats = GetAvailableSeatCount(selectedRow, selectedSeat);
            if (numberOfTickets > remainingSeats) //Check if available seats are enough
            {
                Console.WriteLine();
                Console.WriteLine($"Sorry, there are only {remainingSeats} seats available.");
                return null;
            }

            var booking = new Booking()
            {
                Id = "GIC" + (Bookings.Count + 1).ToString("D4") //Generate a new booking id based on number of confirmed bookings
            };

            if (!selectedRow.HasValue) selectedRow = this.Rows; //By default, start from furthest row from the screen
            int row = selectedRow.Value;
            while (numberOfTickets >= 1 && row >= 1) //Keep looking at next row until all requested seats fulfilled
            {
                if (row == selectedRow && selectedSeat.HasValue) //If this is custom seat selection...
                {
                    int seat = selectedSeat.Value; //Start from the selected seat...
                    while (seat <= SeatsPerRow && numberOfTickets >= 1)  //Look for the next right seat until reaching the end of row
                    {
                        if (!Seats[row - 1, seat - 1]) //Check if this seat is already booked
                        {
                            booking.Seats.Add(new Tuple<int, int>(row, seat));
                            numberOfTickets--;
                        }
                        seat++;
                    }
                }
                else //If this is default seat selection...
                {
                    // Start from the middle-most possible seat...
                    int seat = Math.DivRem(SeatsPerRow, 2).Quotient + Math.DivRem(SeatsPerRow, 2).Remainder;

                    while (seat >= 1 && numberOfTickets >= 1)
                    {
                        if (!Seats[row - 1, seat - 1])
                        {
                            booking.Seats.Add(new Tuple<int, int>(row, seat));
                            numberOfTickets--;
                        }

                        if (numberOfTickets <= 0) break; //Stop if requested number of tickets fulfilled

                        // Look for the seat from other side of the row
                        if (SeatsPerRow - seat + 1 != seat && !Seats[row - 1, SeatsPerRow - seat])
                        {
                            booking.Seats.Add(new Tuple<int, int>(row, SeatsPerRow - seat + 1));
                            numberOfTickets--;
                        }
                        seat--;
                    }
                }
                row--;
            }

            return booking;
        }

        // Print any draft booking or confirmed booking
        public void PrintBooking(Booking booking)
        {
            Console.WriteLine("Booking id: " + booking.Id);
            Console.WriteLine("Selected seats:");
            Console.WriteLine();
            Console.WriteLine("          S C R E E N          ");
            Console.WriteLine("-------------------------------");
            for (int row = 1; row <= Rows; row++)
            {
                Console.WriteLine();
                Console.Write((char)((int)'A' + Rows - row));
                for (int seat = 1; seat <= SeatsPerRow; seat++)
                {
                    Console.Write(seat == 1 ? " " : "  ");
                    if (booking.Seats.Any(s => s.Item1 == row && s.Item2 == seat))
                        Console.Write("o");
                    else
                        Console.Write(Seats[row - 1, seat - 1] ? "#" : ".");
                }
            }
            Console.WriteLine();
            Console.Write(" ");
            for (int seat = 1; seat <= SeatsPerRow; seat++)
                Console.Write((seat == 1 || seat >= 11 ? " " : "  ") + seat);
            Console.WriteLine();
        }

        // Save as real booking after user has confirmed the draft booking created from CreateBooking()
        public void SaveBooking(Booking booking)
        {
            foreach (var seat in booking.Seats)
            {
                Seats[seat.Item1 - 1, seat.Item2 - 1] = true;
            }
            Bookings.Add(booking);
        }

        // Calculate remaining seat count after excluding booked seats
        public int GetAvailableSeatCount(int? selectedRow = null, int? selectedSeat = null)
        {
            if (!selectedRow.HasValue) selectedRow = this.Rows;

            int result = 0;
            for (int row = selectedRow.Value; row >= 1; row--)
            {
                for (int seat = 1; seat <= SeatsPerRow; seat++)
                {
                    if (row != selectedRow) result += Seats[row - 1, seat - 1] ? 0 : 1;
                    else if (!selectedSeat.HasValue || seat >= selectedSeat.Value)
                    {
                        result += Seats[row - 1, seat - 1] ? 0 : 1;
                    }
                }
            }
            return result;
        }

        // Validate and parse seat position from user input string into {row number, seat number}
        public Tuple<int, int>? ParseSeatInput(string? seatInput)
        {
            if (string.IsNullOrWhiteSpace(seatInput)) return null;

            int row = Rows - (int)seatInput.ToUpperInvariant()[0] + (int)'A';
            if (row < 1 || row > Rows)
                return null;

            if (!int.TryParse(seatInput.Substring(1), out int seat) || seat < 1 || seat > SeatsPerRow)
                return null;

            return new Tuple<int, int>(row, seat);
        }

        // Get a booking from saved bookings by booking id
        public Booking? GetBooking(string bookingId)
        {
            return Bookings.FirstOrDefault(b => b.Id == bookingId);
        }
    }
}