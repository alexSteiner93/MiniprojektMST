using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationUpdateTest
        : TestBase
    {
        private readonly ReservationManager _target;

        public ReservationUpdateTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public async Task UpdateReservationTest()
        {

            Reservation reservation = await _target.getReservationByPrimary(1);
            DateTime original = reservation.Bis;
            reservation.Bis = original.AddDays(2);
            _target.UpdateReservation(reservation);

            reservation = await _target.getReservationByPrimary(1);
            Xunit.Assert.Equal(original.AddDays(2), reservation.Bis);
        }
    }
}
