using CinemaBL;
using CinemaDAL.Models;
using System.Diagnostics;

namespace TestProject1
{
    public class Tests
    {
        private CinemaContext _ctx;
        private readonly JobQualificationService _jb;
        private readonly CinemaRoomService _crs;

        public Tests(CinemaContext ctx, CinemaBL.JobQualificationService jb, CinemaBL.CinemaRoomService crs)
        {
            _ctx = ctx;
            _jb = jb;
            _crs  = crs;
        }

        [SetUp]
        public void Setup()
        {
            _ctx = new CinemaDAL.Models.CinemaContext();
        }

        //[TestCase(1,1,1]
        // per passare parametri ai test

        [Test]
        public void GetCinemaRooms_CheckConter_ReturnTrue()
        {

            //var cr = new CinemaBL.CinemaRoomService(Context);
            var ll = _crs.GetCinemaRooms();

            Assert.IsTrue(ll.Count == 0);
            Assert.IsNotNull(ll);
            Assert.IsTrue(ll.Count > 0);

        }

        [Test]
        public void GetJobEmployeeQualification()
        {
            //CinemaBL.JobQualificationService bl = new CinemaBL.JobQualificationService(Context);

            Assert.IsTrue(_jb.GetJobQualifications().Count() > 0);

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