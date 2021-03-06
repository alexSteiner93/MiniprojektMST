﻿using System;
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
            // arrange
            Reservation reservation = await _target.Get(1);
            DateTime original = reservation.Bis;

            // act
            reservation.Bis = original.AddDays(2);
            await _target.Update(reservation);
            reservation = await _target.Get(1);

            // assert
            Xunit.Assert.Equal(original.AddDays(2), reservation.Bis);
        }
    }
}
