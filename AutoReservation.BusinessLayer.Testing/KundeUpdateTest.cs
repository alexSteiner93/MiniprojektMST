﻿using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class KundeUpdateTest
        : TestBase
    {
        private readonly KundeManager _target;

        public KundeUpdateTest()
        {
            _target = new KundeManager();
        }
        
        [Fact]
        public async Task UpdateKundeTest()
        {
            Kunde customer = await _target.Get(1);
            customer.Vorname = "Hansruedi";

            await _target.Update(customer);
            customer = await _target.Get(1);

            Xunit.Assert.Equal("Hansruedi", customer.Vorname);
        }
    }
}
