using CinemaDAL.Models;
using System.Diagnostics;

namespace TestProject1
{
    public class Tests
    {
        private CinemaContext _ctx;


        //private readonly CinemaContext _ctx;

        [SetUp]
        public void Setup()
        {
            _ctx = new CinemaDAL.Models.CinemaContext();
        }

        
        [Test]
        public void Test_GetCinemaRooms()
        {

            var cr = new CinemaBL.CinemaRoomService(_ctx);
            var ll = cr.GetCinemaRooms();

            Assert.IsTrue(ll.Count == 0);
            Assert.IsNotNull(ll);
            Assert.IsTrue(ll.Count > 0);

        }


        // vedi anche
        // https://learn.microsoft.com/it-it/dotnet/core/testing/unit-testing-best-practices?source=recommendations

        //[Theory]
        //[InlineData("", 0)]
        //[InlineData(",", 0)]
        //public void Add_EmptyEntries_ShouldBeTreatedAsZero(string input, int expected)
        //{
        //    // Arrange
        //    var stringCalculator = new StringCalculator();

        //    // Act
        //    var actual = stringCalculator.Add(input);

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}
    }
}