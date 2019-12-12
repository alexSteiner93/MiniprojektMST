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
        private readonly KundeManager _kundeManager;
        private Reservation _existingReservation;
        private Kunde _existingKunde;

        public ReservationAvailabilityTest()
        {
            _target = new ReservationManager();
            _kundeManager = new KundeManager();

        }

        private async Task UpdateContext()
        {
            _existingReservation = await _target.getReservationByPrimary(2);
            _existingKunde = await _kundeManager.getCLientsById(3);
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
            Reservation reservation = new Reservation { Auto = _existingReservation.Auto, Kunde = _existingKunde, Von = von, Bis = bis };

            // assert
            _target.AddReservation(reservation);
        }

        [Fact]
        public async Task ScenarioOkay02Test()
        {
            // arrange
            //| ---Date 1--- |
            //                 | ---Date 2--- |
            await UpdateContext();
            DateTime von = _existingReservation.Bis.AddDays(2);
            DateTime bis = von.AddDays(2);

            // act
            Reservation reservation = new Reservation { Auto = _existingReservation.Auto, Kunde = _existingKunde, Von = von, Bis = bis };

            // assert
            _target.AddReservation(reservation);
        }

        [Fact]
        public async Task ScenarioOkay03Test()
        {
            // arrange
            //                | ---Date 1--- |
            //| ---Date 2-- - |
            await UpdateContext();
            DateTime bis = _existingReservation.Bis.AddDays(-1);
            DateTime von = bis.AddDays(-2);

            // act
            Reservation reservation = new Reservation { Auto =  _existingReservation.Auto, Kunde = _existingKunde, Von = von, Bis = bis };

            // assert
            _target.AddReservation(reservation);
        }

        [Fact]
        public async Task ScenarioOkay04Test()
        {
            // arrange
            //                | ---Date 1--- |
            //| ---Date 2--- |
            await UpdateContext();
            DateTime bis = _existingReservation.Bis.AddDays(-2);
            DateTime von = bis.AddDays(-2);

            // act
            Reservation reservation = new Reservation { Auto = _existingReservation.Auto, Kunde =_existingKunde, Von = von, Bis = bis };

            // assert
            _target.AddReservation(reservation);
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
            Reservation reservation = new Reservation { Auto = _existingReservation.Auto, Kunde = _existingKunde, Von = von, Bis = bis };
            Action reservate = () => _target.AddReservation(reservation);

            // assert
            Xunit.Assert.Throws<AutoUnavailableException>(reservate);
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
            Reservation reservation = new Reservation { Auto = _existingReservation.Auto, Kunde = _existingKunde, Von = von, Bis = bis };
            Action reservate = () => _target.AddReservation(reservation);

            // assert
            Xunit.Assert.Throws<AutoUnavailableException>(reservate);
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
            Reservation reservation = new Reservation { Auto = _existingReservation.Auto, Kunde = _existingKunde, Von = von, Bis = bis };
            Action reservate = () => _target.AddReservation(reservation);

            // assert
            Xunit.Assert.Throws<AutoUnavailableException>(reservate);
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
            Reservation reservation = new Reservation { Auto = _existingReservation.Auto, Kunde = _existingKunde, Von = von, Bis = bis };
            Action reservate = () => _target.AddReservation(reservation);

            // assert
            Xunit.Assert.Throws<AutoUnavailableException>(reservate);
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
            Reservation reservation = new Reservation { Auto = _existingReservation.Auto, Kunde =_existingKunde, Von = von, Bis = bis };
            Action reservate = () => _target.AddReservation(reservation);

            // assert
            Xunit.Assert.Throws<AutoUnavailableException>(reservate);
        }
    }
}
