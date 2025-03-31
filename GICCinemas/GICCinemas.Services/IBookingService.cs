namespace GICCinemas.Services
{
    public interface IBookingService
    {
        void InitData();
        void BookTicket();
        void CheckBooking();
        int GetAvailableSeatCount();
        string GetMovieTitle();
    }
}
