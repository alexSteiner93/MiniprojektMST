using System;
using System.Threading.Tasks;
using AutoReservation.Service.Grpc.Testing.Common;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Xunit;

namespace AutoReservation.Service.Grpc.Testing
{
    public class ReservationServiceTests
        : ServiceTestBase
    {
        private readonly ReservationService.ReservationServiceClient _target;
        private readonly AutoService.AutoServiceClient _autoClient;
        private readonly KundeService.KundeServiceClient _kundeClient;

        public ReservationServiceTests(ServiceTestFixture serviceTestFixture)
            : base(serviceTestFixture)
        {
            _target = new ReservationService.ReservationServiceClient(Channel);
            _autoClient = new AutoService.AutoServiceClient(Channel);
            _kundeClient = new KundeService.KundeServiceClient(Channel);
        }

        [Fact]
        public async Task GetReservationenTest()
        {
            ReservationAllDto result =  _target.GetAll(new Empty());
            RepeatedField<ReservationDto> reservationDtos = result.Reservations;
            Assert.Equal(4, reservationDtos.Count);
        }

        [Fact]
        public async Task GetReservationByIdTest()
        {
            ReservationDto result = _target.Get(new ReservationRequest { Id = 1 });

            Assert.Equal(result.Auto.Tagestarif, 50);
        }

        [Fact]
        public async Task GetReservationByIdWithIllegalIdTest()
        {
            ReservationDto result = _target.Get(new ReservationRequest { Id = -1 });

            Assert.Throws<RpcException>(() => _target.Get(result));
        }

        [Fact]
        public async Task InsertReservationTest()
        {
            KundeDto client = new KundeDto { Nachname = "Weber", Vorname = "Franz" };
            AutoDto car = new AutoDto();
            car.Marke = "Ferrari";
            car.Tagestarif = 10000;
            car.AutoKlasse = AutoKlasse.Luxusklasse;
            car.Basistarif = 100;
            Timestamp von = Timestamp.FromDateTime(new DateTime(2019, 12, 23, 0, 0, 0, DateTimeKind.Utc));
            Timestamp bis = Timestamp.FromDateTime(new DateTime(2019, 12, 26, 0, 0, 0, DateTimeKind.Utc));
            ReservationDto result = new ReservationDto();
            result.Von = von;
            result.Bis = bis;
            result.Auto = car;
            result.Kunde = client;

            ReservationDto backDto = _target.Insert(result);

            Assert.Equal(_target.Get(new ReservationRequest { Id = backDto.ReservationsNr }).Tagestarif, 10000);


        }

        [Fact]
        public async Task DeleteReservationTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task UpdateReservationTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task UpdateReservationWithOptimisticConcurrencyTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task InsertReservationWithInvalidDateRangeTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task InsertReservationWithAutoNotAvailableTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task UpdateReservationWithInvalidDateRangeTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task UpdateReservationWithAutoNotAvailableTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task CheckAvailabilityIsTrueTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task CheckAvailabilityIsFalseTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }
    }
}