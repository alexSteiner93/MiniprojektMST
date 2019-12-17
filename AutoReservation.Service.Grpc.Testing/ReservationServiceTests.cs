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
            ReservationDto toDelete = _target.Get(new ReservationRequest { Id = 1 });
            _target.Delete(toDelete);

            Assert.Throws<RpcException>(() => _target.Get(toDelete));
        }

        [Fact]
        public async Task UpdateReservationTest()
        {
            string newModel = "asdf";
            ReservationDto toUpdate = _target.Get(new ReservationRequest { Id = 1 });
            toUpdate.Auto.Marke = newModel;
            _target.Update(toUpdate);

            Assert.Equal(newModel, _target.Get(toUpdate).Marke);


        }

        [Fact]
        public async Task UpdateReservationWithOptimisticConcurrencyTest()
        {
            ReservationDto toUpdate = _target.Get(new ReservationRequest { Id = 1 });
            AutoDto concurrentReservation = _target.Get(toUpdate);

            // act
            _target.Update(concurrentReservation);

            // assert
            Assert.Throws<RpcException>(() => _target.Update(toUpdate));
        }

        [Fact]
        public async Task InsertReservationWithInvalidDateRangeTest()
        {
         
            KundeDto kundeDto = _kundeClient.Get(new KundeRequest { Id = 2 });
            AutoDto autoDto = _autoClient.Get(new AutoRequest { Id = 2 });
            Timestamp von = Timestamp.FromDateTime(new DateTime(2020, 4, 10, 00, 00, 00, DateTimeKind.Utc));
            Timestamp bis = Timestamp.FromDateTime(new DateTime(2002, 10, 05, 00, 00, 00, DateTimeKind.Utc));

            ReservationDto reservationInvalid = new ReservationDto();
            reservationInvalid.Von = von;
            reservationInvalid.Bis = bis;
            reservationInvalid.Kunde = kundeDto;
            reservationInvalid.Auto = autoDto;

            Assert.Throws<RpcException>(() => _target.Insert(reservationInvalid));
        }

        [Fact]
        public async Task InsertReservationWithAutoNotAvailableTest()
        {
            ReservationDto reservationInvalid = new ReservationDto();
            Timestamp Von = Timestamp.FromDateTime(new DateTime(2019, 12, 13, 0, 0, 0, DateTimeKind.Utc));
            Timestamp Bis = Timestamp.FromDateTime(new DateTime(2019, 12, 20, 0, 0, 0, DateTimeKind.Utc));
            KundeDto Client = _kundeClient.Get(new KundeRequest { Id = 4 });
            AutoDto Car = _autoClient.Get(new AutoRequest { Id = 4 });

            reservationInvalid.Von = Von;
            reservationInvalid.Bis = Bis;
            reservationInvalid.Kunde = Client;
            reservationInvalid.Auto = Car;

            Assert.Throws<RpcException>(() => _target.Insert(reservationInvalid));

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