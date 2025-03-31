using GICCinemas.Services;
using System.Diagnostics.Metrics;

namespace GICCinemas.Test
{
    public class BookingServiceTest
    {
        private readonly IBookingService bookingService;
        public BookingServiceTest(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        [Fact]
        public void TestInitData_Successful()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            Console.SetIn(new StringReader("Inception 8 10"));
            bookingService.InitData();
            string title = bookingService.GetMovieTitle();
            int seatCount = bookingService.GetAvailableSeatCount();

            // Assert
            Assert.Contains("Please define movie title and seating map in [Title] [Row] [SeatsPerRow] format:", writer.ToString());
            Assert.Equal("Inception", title);
            Assert.Equal(80, seatCount);
        }

        [Fact]
        public void TestInitData_Validation_Format()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            Console.SetIn(new StringReader("Inception 10\nInception 8 10"));
            bookingService.InitData();
            string title = bookingService.GetMovieTitle();
            int seatCount = bookingService.GetAvailableSeatCount();

            // Assert
            Assert.Contains("Please input in correct format [Title] [Row] [SeatsPerRow]:", writer.ToString());
            Assert.Equal("Inception", title);
            Assert.Equal(80, seatCount);
        }

        [Fact]
        public void TestInitData_Validation_Row()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            Console.SetIn(new StringReader("Inception 27 50\nInception 8 10"));
            bookingService.InitData();
            string title = bookingService.GetMovieTitle();
            int seatCount = bookingService.GetAvailableSeatCount();

            // Assert
            Assert.Contains("Row must be a valid positive number <= 26", writer.ToString());
            Assert.Equal("Inception", title);
            Assert.Equal(80, seatCount);
        }

        [Fact]
        public void TestInitData_Validation_SeatsPerRow()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            Console.SetIn(new StringReader("Inception 26 51\nInception 8 10"));
            bookingService.InitData();
            string title = bookingService.GetMovieTitle();
            int seatCount = bookingService.GetAvailableSeatCount();

            // Assert
            Assert.Contains("SeatsPerRow must be a valid positive number <= 50", writer.ToString());
            Assert.Equal("Inception", title);
            Assert.Equal(80, seatCount);
        }

        [Fact]
        public void TestBookTicket_NotEnoughSeats()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Console.SetIn(new StringReader("Inception 8 10"));
            bookingService.InitData();

            // Act
            Console.SetIn(new StringReader("81\n"));
            bookingService.BookTicket();

            // Assert
            Assert.Contains("Sorry, there are only 80 seats available.", writer.ToString());
        }

        [Fact]
        public void TestBookTicket_Successful()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Console.SetIn(new StringReader("Inception 8 10"));
            bookingService.InitData();
            writer.Flush();

            // Act
            Console.SetIn(new StringReader("4\n"));
            bookingService.BookTicket();

            // Assert
            Assert.Contains("Enter number of tickets to book, or enter blank to go back to main menu:", writer.ToString());
            Assert.Contains("Successfully reserved 4 Inception tickets.", writer.ToString());
            Assert.Contains("Booking id: GIC0001 confirmed.", writer.ToString());
        }

        [Fact]
        public void TestBookTicket_CustomSeat_Successful()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Console.SetIn(new StringReader("Inception 8 10"));
            bookingService.InitData();
            writer.Flush();

            // Act
            Console.SetIn(new StringReader("4\nB03\n"));
            bookingService.BookTicket();

            // Assert
            Assert.Contains("Successfully reserved 4 Inception tickets.", writer.ToString());
            Assert.Contains("Booking id: GIC0001 confirmed.", writer.ToString());
        }

        [Fact]
        public void TestBookTicket_CustomSeat_Validation()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Console.SetIn(new StringReader("Inception 8 10"));
            bookingService.InitData();
            writer.Flush();

            // Act
            Console.SetIn(new StringReader("4\nB11\nB03\n"));
            bookingService.BookTicket();

            // Assert
            Assert.Contains("Please enter a valid seat:", writer.ToString());
            Assert.Contains("Successfully reserved 4 Inception tickets.", writer.ToString());
            Assert.Contains("Booking id: GIC0001 confirmed.", writer.ToString());
        }

        [Fact]
        public void TestCheckBooking_Successful()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Console.SetIn(new StringReader("Inception 8 10"));
            bookingService.InitData();
            Console.SetIn(new StringReader("4\n"));
            bookingService.BookTicket();
            writer.Flush();

            // Act
            Console.SetIn(new StringReader("GIC0001"));
            bookingService.CheckBooking();

            // Assert
            Assert.Contains("Booking id: GIC0001", writer.ToString());
        }

        [Fact]
        public void TestCheckBooking_NotFound()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Console.SetIn(new StringReader("Inception 8 10"));
            bookingService.InitData();
            Console.SetIn(new StringReader("4\n"));
            bookingService.BookTicket();
            writer.Flush();

            // Act
            Console.SetIn(new StringReader("GIC0002"));
            bookingService.CheckBooking();

            // Assert
            Assert.Contains("Cannot find booking id GIC0002", writer.ToString());
        }

        [Fact]
        public void TestBook2ndTicket_Successful()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Console.SetIn(new StringReader("Inception 8 10"));
            bookingService.InitData();
            Console.SetIn(new StringReader("4\n"));
            bookingService.BookTicket();
            writer.Flush();

            // Act
            Console.SetIn(new StringReader("4\n"));
            bookingService.BookTicket();

            // Assert
            Assert.Contains("Enter number of tickets to book, or enter blank to go back to main menu:", writer.ToString());
            Assert.Contains("Successfully reserved 4 Inception tickets.", writer.ToString());
            Assert.Contains("Booking id: GIC0002 confirmed.", writer.ToString());
        }

        [Fact]
        public void TestCheck2ndBooking_Successful()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Console.SetIn(new StringReader("Inception 8 10"));
            bookingService.InitData();
            Console.SetIn(new StringReader("4\n"));
            bookingService.BookTicket();
            Console.SetIn(new StringReader("4\n"));
            bookingService.BookTicket();
            writer.Flush();

            // Act
            Console.SetIn(new StringReader("GIC0002"));
            bookingService.CheckBooking();

            // Assert
            Assert.Contains("Booking id: GIC0002", writer.ToString());
        }
    }
}