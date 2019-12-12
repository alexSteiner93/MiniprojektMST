using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class AutoUpdateTests
        : TestBase
    {
        private readonly AutoManager _target;

        public AutoUpdateTests()
        {
            _target = new AutoManager();
        }

        [Fact]
        public async Task UpdateAutoTest()
        {
            Auto car = await _target.GetCarByPrimary(1);
            car.Tagestarif = 60;

            _target.UpdateCar(car);

            car = await _target.GetCarByPrimary(1);
            Xunit.Assert.Equal(60, car.Tagestarif);
        }
    }
}
