namespace GICCinemas.Services
{
    public interface ISeatRepository
    {
        void InitData(int row, int seatsPerRow);
        Booking? CreateBooking(int numberOfTickets, int? selectedRow = null, int? selectedSeat = null);
        void PrintBooking(Booking booking);
        void SaveBooking(Booking booking);
        Booking? GetBooking(string bookingId);
        int GetAvailableSeatCount(int? selectedRow = null, int? selectedSeat = null);
        Tuple<int, int>? ParseSeatInput(string? seatInput);
    }
}
