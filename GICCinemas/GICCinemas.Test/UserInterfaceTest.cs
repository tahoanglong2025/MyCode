using GICCinemas.Services;

namespace GICCinemas.Test
{
    public class UserInterfaceTest
    {
        private readonly IUserInterface userInterface;
        public UserInterfaceTest(IUserInterface userInterface)
        {
            this.userInterface = userInterface;
        }

        [Fact]
        public void TestInitData_DisplayMenu_Successful()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Console.SetIn(new StringReader("Inception 8 10"));
            userInterface.InitData();
            writer.Flush();

            // Act
            Console.SetIn(new StringReader("3"));
            userInterface.DisplayMenu();

            // Assert
            Assert.Contains("Welcome to GIC Cinemas", writer.ToString());
            Assert.Contains("Book tickets for Inception (80 seats available)", writer.ToString());
            Assert.Contains("Thank you for using GIC Cinemas system. Bye!", writer.ToString());
        }

        [Fact]
        public void TestInitData_BookTicket_Successful()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Console.SetIn(new StringReader("Inception 8 10"));
            userInterface.InitData();
            writer.Flush();

            // Act
            Console.SetIn(new StringReader("1\n4\n"));
            userInterface.DisplayMenu();

            // Assert
            Assert.Contains("Enter number of tickets to book, or enter blank to go back to main menu:", writer.ToString());
            Assert.Contains("Successfully reserved 4 Inception tickets.", writer.ToString());
            Assert.Contains("Booking id: GIC0001 confirmed.", writer.ToString());
        }

        [Fact]
        public void TestInitData_BookTicket_CustomSeat_Successful()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Console.SetIn(new StringReader("Inception 8 10"));
            userInterface.InitData();
            writer.Flush();

            // Act
            Console.SetIn(new StringReader("1\n4\nB03\n"));
            userInterface.DisplayMenu();

            // Assert
            Assert.Contains("Successfully reserved 4 Inception tickets.", writer.ToString());
            Assert.Contains("Booking id: GIC0001 confirmed.", writer.ToString());
        }

        [Fact]
        public void TestInitData_Book2ndTicket_Successful()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Console.SetIn(new StringReader("Inception 8 10"));
            userInterface.InitData();
            Console.SetIn(new StringReader("1\n4\n"));
            userInterface.DisplayMenu();
            writer.Flush();

            // Act
            Console.SetIn(new StringReader("1\n4\n"));
            userInterface.DisplayMenu();

            // Assert
            Assert.Contains("Successfully reserved 4 Inception tickets.", writer.ToString());
            Assert.Contains("Booking id: GIC0002 confirmed.", writer.ToString());
        }

        [Fact]
        public void TestInitData_CheckTicket_Successful()
        {
            // Arrange
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Console.SetIn(new StringReader("Inception 8 10"));
            userInterface.InitData();
            writer.Flush();

            // Act
            Console.SetIn(new StringReader("1\n4\n2\nGIC0001\n"));
            userInterface.DisplayMenu();

            // Assert
            Assert.Contains("Booking id: GIC0001", writer.ToString());
        }
    }
}