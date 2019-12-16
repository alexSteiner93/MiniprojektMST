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

            AutosDto result = _target.GetAllCars(new Empty());

            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async Task GetAutoByIdTest()
        {
            AutoDto result = _target.Get(new AutoRequest { Id = 1 });

            Assert.Equal(result.Tagestarif, 50);
        }

        [Fact]
        public async Task GetAutoByIdWithIllegalIdTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task InsertAutoTest()
        {
            AutoDto result = new AutoDto();
            result.Marke = "Ferrari";
            result.Tagestarif = 10000;
            result.AutoKlasse = AutoKlasse.Luxusklasse;
            result.Basistarif = 100;

            _target.AddCar();

            Assert.Equal(_target.GetCarByPrimary(5).Tagestarif, 10000);
        }

        [Fact]
        public async Task DeleteAutoTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task UpdateAutoTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task UpdateAutoWithOptimisticConcurrencyTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }
    }
}