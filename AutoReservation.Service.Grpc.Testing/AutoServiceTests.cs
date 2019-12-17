using System;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using AutoReservation.Service.Grpc.Testing.Common;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Xunit;

namespace AutoReservation.Service.Grpc.Testing
{
    public class AutoServiceTests
        : ServiceTestBase
    {
        private readonly AutoService.AutoServiceClient _target;

        public AutoServiceTests(ServiceTestFixture serviceTestFixture)
            : base(serviceTestFixture)
        {
            _target = new AutoService.AutoServiceClient(Channel);
        }


        [Fact]
        public async Task GetAutosTest()
        {
            AutoAllDto result = _target.GetAll(new Empty());
            RepeatedField<AutoDto> autoDtos = result.Cars;

            Assert.Equal(4, autoDtos.Count);
        }

        [Fact]
        public async Task GetAutoByIdTest()
        {
            AutoDto result =  _target.Get(new AutoRequest { Id = 1 });

            Assert.Equal(50, result.Tagestarif);
        }

        [Fact]
        public async Task GetAutoByIdWithIllegalIdTest()
        {
            // arrange
            AutoRequest toGet = new AutoRequest { Id = -1 };

            // act

            // assert
            Assert.Throws<RpcException>(() => _target.Get(toGet));
        }

        [Fact] 
        public async Task InsertAutoTest()
        {
            // arrange
            AutoDto car = new AutoDto();
            car.Marke = "Ferrari";
            car.Tagestarif = 10000;
            car.AutoKlasse = AutoKlasse.Luxusklasse;
            car.Basistarif = 100;

            // act
            AutoDto newCar = _target.Insert(car);
            AutoDto actual = _target.Get(new AutoRequest { Id = newCar.Id });

            Assert.Equal(10000, actual.Tagestarif);
        }

        [Fact]
        public async Task DeleteAutoTest()
        {
            // arrange
            AutoRequest toDelete = new AutoRequest { Id = 1 };
            AutoDto car = _target.Get(toDelete);

            // act
            _target.Delete(car);

            // assert
            Assert.Throws<RpcException>(() => _target.Get(toDelete));
        }

        [Fact]
        public async Task UpdateAutoTest()
        {
            // arrange
            string newModel = "asdf";
            AutoRequest toUpdate = new AutoRequest { Id = 1 };
            AutoDto car = _target.Get(toUpdate);

            // act
            car.Marke = newModel;
            _target.Update(car);

            // assert
            Assert.Equal(newModel,  _target.Get(toUpdate).Marke);
        }

        [Fact]
        public async Task UpdateAutoWithOptimisticConcurrencyTest()
        {
            // arrange
            AutoRequest toUpdate = new AutoRequest { Id = 1 };
            AutoDto car = _target.Get(toUpdate);
            AutoDto concurrentCar = _target.Get(toUpdate);

            // act
            _target.Update(concurrentCar);

            // assert
            Assert.Throws<RpcException>(() =>  _target.Update(car));
        }
    }
}