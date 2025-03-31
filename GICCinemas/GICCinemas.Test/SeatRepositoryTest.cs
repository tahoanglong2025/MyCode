using GICCinemas.Services;

namespace GICCinemas.Test
{
    public class SeatRepositoryTest
    {
        private readonly ISeatRepository seatRepository;
        public SeatRepositoryTest(ISeatRepository seatRepository)
        {
            this.seatRepository = seatRepository;
        }

        [Fact]
        public void TestGetAvailableSeatCount()
        {
            // Arrange
            seatRepository.InitData(8, 10);

            // Act
            int availableSeatCount = seatRepository.GetAvailableSeatCount();

            // Assert
            Assert.Equal(80, availableSeatCount);
        }

        [Fact]
        public void TestGetAvailableSeatCount_GotBooking()
        {
            // Arrange
            seatRepository.InitData(8, 10);
            var booking = seatRepository.CreateBooking(4);
            Assert.NotNull(booking);
            seatRepository.SaveBooking(booking);

            // Act
            int availableSeatCount = seatRepository.GetAvailableSeatCount();

            // Assert
            Assert.Equal(76, availableSeatCount);
        }

        [Fact]
        public void TestCreateBooking_Default_Successful()
        {
            // Arrange
            seatRepository.InitData(8, 10);

            // Act
            var booking = seatRepository.CreateBooking(4);

            // Assert
            Assert.NotNull(booking);
            Assert.Equal("GIC0001", booking.Id);
            Assert.Equal(4, booking.Seats.Count);
        }

        [Fact]
        public void TestCreateBooking_Custom_Successful()
        {
            // Arrange
            seatRepository.InitData(8, 10);

            // Act
            var booking = seatRepository.CreateBooking(4, 2, 3);

            // Assert
            Assert.NotNull(booking);
            Assert.Equal("GIC0001", booking.Id);
            Assert.Equal(4, booking.Seats.Count);
            Assert.Equal(2, booking.Seats[0].Item1);
            Assert.Equal(3, booking.Seats[0].Item2);
        }

        [Fact]
        public void TestParseSeatInput()
        {
            // Arrange
            seatRepository.InitData(8, 10);

            // Act and Assert
            var parsedSeat = seatRepository.ParseSeatInput("");
            Assert.Null(parsedSeat);
            parsedSeat = seatRepository.ParseSeatInput("abc");
            Assert.Null(parsedSeat);
            parsedSeat = seatRepository.ParseSeatInput("a0");
            Assert.Null(parsedSeat);
            parsedSeat = seatRepository.ParseSeatInput("z1");
            Assert.Null(parsedSeat);
            parsedSeat = seatRepository.ParseSeatInput("a10");
            Assert.NotNull(parsedSeat);
            Assert.Equal(8, parsedSeat.Item1);
            Assert.Equal(10, parsedSeat.Item2);
            parsedSeat = seatRepository.ParseSeatInput("B03");
            Assert.NotNull(parsedSeat);
            Assert.Equal(7, parsedSeat.Item1);
            Assert.Equal(3, parsedSeat.Item2);
            parsedSeat = seatRepository.ParseSeatInput("H5");
            Assert.NotNull(parsedSeat);
            Assert.Equal(1, parsedSeat.Item1);
            Assert.Equal(5, parsedSeat.Item2);
        }

        [Fact]
        public void TestSaveBooking_GetBooking_Successful()
        {
            // Arrange
            seatRepository.InitData(8, 10);
            var booking = seatRepository.CreateBooking(4);
            Assert.NotNull(booking);

            // Act
            seatRepository.SaveBooking(booking);
            var retrievedBooking = seatRepository.GetBooking(booking.Id);

            // Assert
            Assert.NotNull(retrievedBooking);
            Assert.Equal(booking, retrievedBooking);
        }

        [Fact]
        public void TestSaveBooking_PrintBooking_Successful()
        {
            // Arrange
            seatRepository.InitData(8, 10);
            var booking = seatRepository.CreateBooking(4);
            Assert.NotNull(booking);

            // Act
            seatRepository.SaveBooking(booking);
            seatRepository.PrintBooking(booking);
        }
    }
}