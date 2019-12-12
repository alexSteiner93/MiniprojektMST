using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationDateRangeTest
        : TestBase
    {
        private readonly ReservationManager _target;

        public ReservationDateRangeTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public void ScenarioOkay01Test()
        {
            // arrange
            DateTime von = new DateTime(2019, 1, 1);
            DateTime bis = new DateTime(2019, 1, 10);

            // act
            bool IsCorrect = _target.IsDateCorrect(von, bis);

            // assert
            Xunit.Assert.True(IsCorrect);
        }

        [Fact]
        public void ScenarioOkay02Test()
        {
            // arrange
            DateTime von = new DateTime(2019, 1, 1);
            DateTime bis = new DateTime(2019, 1, 2);

            // act
            bool IsCorrect = _target.IsDateCorrect(von, bis);

            // assert
            Xunit.Assert.True(IsCorrect);
        }

        [Fact]
        public void ScenarioNotOkay01Test()
        {
            // arrange
            DateTime von = new DateTime(2019, 1, 1);
            DateTime bis = new DateTime(2019, 1, 1);

            // act
            bool IsCorrect = _target.IsDateCorrect(von, bis);

            // assert
            Xunit.Assert.False(IsCorrect);
        }

        [Fact]
        public void ScenarioNotOkay02Test()
        {
            // arrange
            DateTime von = new DateTime(2019, 1, 2);
            DateTime bis = new DateTime(2019, 1, 1);

            // act
            bool IsCorrect = _target.IsDateCorrect(von, bis);

            // assert
            Xunit.Assert.False(IsCorrect);
        }

        [Fact]
        public void ScenarioNotOkay03Test()
        {
            // arrange
            DateTime von = new DateTime(2019, 1, 2);
            DateTime bis = new DateTime(2019, 1, 1);

            // act
            bool IsCorrect = _target.IsDateCorrect(von, bis);

            // assert
            Xunit.Assert.False(IsCorrect);
        }
    }
}
