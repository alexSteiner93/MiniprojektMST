using System;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;
using Xunit.Abstractions;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationAvailabilityTest
        : TestBase
    {
        private readonly ReservationManager _target;
        private Reservation _existingReservation;

        public ReservationAvailabilityTest()
        {
            _target = new ReservationManager();

        }

        private async Task UpdateContext()
        {
            _existingReservation = await _target.getReservationByPrimary(1);
        }

        [Fact]
        public async Task ScenarioOkay01Test()
        {
            // arrange
            //| ---Date 1--- |
            //               | ---Date 2--- |
            await UpdateContext();
            DateTime von = _existingReservation.Bis.AddDays(1);
            DateTime bis = von.AddDays(2);

            // act
            bool IsAvailable = _target.IsCarAvailable(_existingReservation.AutoId, von, bis);

            // assert
            Xunit.Assert.True(IsAvailable);
        }

        [Fact]
        public async Task ScenarioOkay02Test()
        {
            // arrange
            //| ---Date 1--- |
            //                 | ---Date 2--- |
            await UpdateContext();
            DateTime von = _existingReservation.Bis.AddDays(1);
            DateTime bis = von.AddDays(2);

            // act
            bool IsAvailable = _target.IsCarAvailable(_existingReservation.AutoId, von, bis);

            // assert
            Xunit.Assert.True(IsAvailable);
        }

        [Fact]
        public async Task ScenarioOkay03Test()
        {
            // arrange
            //                | ---Date 1--- |
            //| ---Date 2-- - |
            await UpdateContext();
            DateTime bis = _existingReservation.Von.AddDays(-1);
            DateTime von = bis.AddDays(-2);

            // act
            bool IsAvailable = _target.IsCarAvailable(_existingReservation.AutoId, von, bis);

            // assert
            Xunit.Assert.True(IsAvailable);
        }

        [Fact]
        public async Task ScenarioOkay04Test()
        {
            // arrange
            //                | ---Date 1--- |
            //| ---Date 2--- |
            await UpdateContext();
            DateTime bis = _existingReservation.Von.AddDays(-1);
            DateTime von = bis.AddDays(-2);

            // act
            bool IsAvailable = _target.IsCarAvailable(_existingReservation.AutoId, von, bis);

            // assert
            Xunit.Assert.True(IsAvailable);
        }

        [Fact]
        public async Task ScenarioNotOkay01Test()
        {
            // arrange
            //| ---Date 1--- |
            //    | ---Date 2--- |
            await UpdateContext();
            DateTime von = _existingReservation.Von.AddDays(2);
            DateTime bis = _existingReservation.Bis.AddDays(2);

            // act
            bool IsAvailable = _target.IsCarAvailable(_existingReservation.AutoId, von, bis);

            // assert
            Xunit.Assert.False(IsAvailable);
        }

        [Fact]
        public async Task ScenarioNotOkay02Test()
        {
            // arrange
            //    | ---Date 1--- |
            //| ---Date 2--- |
            await UpdateContext();
            DateTime von = _existingReservation.Von.AddDays(-2);
            DateTime bis = _existingReservation.Bis.AddDays(-2);

            // act
            bool IsAvailable = _target.IsCarAvailable(_existingReservation.AutoId, von, bis);

            // assert
            Xunit.Assert.False(IsAvailable);
        }

        [Fact]
        public async Task ScenarioNotOkay03Test()
        {
            // arrange
            //| ---Date 1--- |
            //| --------Date 2-------- |
            await UpdateContext();
            DateTime von = _existingReservation.Von;
            DateTime bis = _existingReservation.Bis.AddDays(2);

            // act
            bool IsAvailable = _target.IsCarAvailable(_existingReservation.AutoId, von, bis);

            // assert
            Xunit.Assert.False(IsAvailable);
        }

        [Fact]
        public async Task ScenarioNotOkay04Test()
        {
            // arrange
            //| --------Date 1-------- |
            //| ---Date 2--- |
            await UpdateContext();
            DateTime von = _existingReservation.Von;
            DateTime bis = _existingReservation.Bis.AddDays(-1);

            // act
            bool IsAvailable = _target.IsCarAvailable(_existingReservation.AutoId, von, bis);

            // assert
            Xunit.Assert.False(IsAvailable);
        }

        [Fact]
        public async Task ScenarioNotOkay05Test()
        {
            // arrange
            //| ---Date 1--- |
            //| ---Date 2--- |
            await UpdateContext();
            DateTime von = _existingReservation.Von;
            DateTime bis = _existingReservation.Bis;

            // act
            bool IsAvailable = _target.IsCarAvailable(_existingReservation.AutoId, von, bis);

            // assert
            Xunit.Assert.False(IsAvailable);
        }
    }
}
