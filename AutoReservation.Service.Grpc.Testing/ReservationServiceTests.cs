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

            Assert.Equal(50, result.Auto.Tagestarif);
        }

        [Fact]
        public async Task GetReservationByIdWithIllegalIdTest()
        {
            // arrange
            ReservationRequest toGet = new ReservationRequest { Id = -1 };

            // assert
            Assert.Throws<RpcException>(() => _target.Get(toGet));
        }

        [Fact]
        public async Task InsertReservationTest()
        {
            // arrange
            KundeDto client = _kundeClient.Get(new KundeRequest { Id = 1 });
            AutoDto car = _autoClient.Get(new AutoRequest { Id = 1 });
           
            Timestamp von = Timestamp.FromDateTime(new DateTime(2019, 12, 23, 0, 0, 0, DateTimeKind.Utc));
            Timestamp bis = Timestamp.FromDateTime(new DateTime(2019, 12, 26, 0, 0, 0, DateTimeKind.Utc));
            
            // act
            ReservationDto reservation = new ReservationDto {Auto = car, Kunde = client, Von = von, Bis = bis };
            ReservationDto result = _target.Insert(reservation);
            
            // assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteReservationTest()
        {
            // arrange
            ReservationRequest toDelete = new ReservationRequest { Id = 1 };
            ReservationDto reservation = _target.Get(toDelete);

            // act
            _target.Delete(reservation);

            // assert
            Assert.Throws<RpcException>(() => _target.Get(toDelete));
        }

        [Fact]
        public async Task UpdateReservationTest()
        {
            // arrange
            ReservationRequest toUpdate = new ReservationRequest { Id = 1 };
            ReservationDto reservation = _target.Get(toUpdate);
            DateTime bis = reservation.Bis.ToDateTime();

            // act
            reservation.Bis = bis.AddDays(1).ToTimestamp();
            _target.Update(reservation);

            // assert
            Assert.Equal(bis.AddDays(1).ToTimestamp(), _target.Get(toUpdate).Bis);
        }

        [Fact]
        public async Task UpdateReservationWithOptimisticConcurrencyTest()
        {
            // arrange
            ReservationRequest toUpdate = new ReservationRequest { Id = 1 };
            ReservationDto reservation = _target.Get(toUpdate);
            ReservationDto concurrentReservation = _target.Get(toUpdate);

            // act
            _target.Update(concurrentReservation);

            // assert
            Assert.Throws<RpcException>(() => _target.Update(reservation));
        }

        [Fact]
        public async Task InsertReservationWithInvalidDateRangeTest()
        {
            // arrange 
            KundeDto kundeDto = _kundeClient.Get(new KundeRequest { Id = 2 });
            AutoDto autoDto = _autoClient.Get(new AutoRequest { Id = 2 });
            Timestamp von = Timestamp.FromDateTime(new DateTime(2020, 4, 10, 00, 00, 00, DateTimeKind.Utc));
            Timestamp bis = Timestamp.FromDateTime(new DateTime(2002, 10, 05, 00, 00, 00, DateTimeKind.Utc));

            ReservationDto reservationInvalid = new ReservationDto();
            reservationInvalid.Von = von;
            reservationInvalid.Bis = bis;
            reservationInvalid.Kunde = kundeDto;
            reservationInvalid.Auto = autoDto;

            // assert
            Assert.Throws<RpcException>(() => _target.Insert(reservationInvalid));
        }

        [Fact]
        public async Task InsertReservationWithAutoNotAvailableTest()
        {
            // arrange
            ReservationRequest existingRequest = new ReservationRequest { Id = 1 };
            ReservationDto existingDto = _target.Get(existingRequest);
            
            ReservationDto reservationInvalid = new ReservationDto {Auto = existingDto.Auto, Kunde = existingDto.Kunde, Von = existingDto.Von, Bis = existingDto.Bis };


            // assert
            Assert.Throws<RpcException>(() => _target.Insert(reservationInvalid));
        }

        [Fact]
        public async Task UpdateReservationWithInvalidDateRangeTest()
        {
            // arrange
            ReservationRequest existingRequest = new ReservationRequest { Id = 1 };
            ReservationDto existingDto = _target.Get(existingRequest);
            Timestamp von = existingDto.Bis;
            Timestamp bis = existingDto.Von;

            existingDto.Von = von;
            existingDto.Bis = bis;

            // assert
            Assert.Throws<RpcException>(() => _target.Update(existingDto));
        }

        [Fact]
        public async Task UpdateReservationWithAutoNotAvailableTest()
        {
            // arrange
            ReservationRequest existingRequest = new ReservationRequest { Id = 1 };
            ReservationDto existingDto = _target.Get(existingRequest);

            ReservationDto reservationInvalid = new ReservationDto { Auto = existingDto.Auto, Kunde = existingDto.Kunde, Von = existingDto.Von, Bis = existingDto.Bis };


            // assert
            Assert.Throws<RpcException>(() => _target.Insert(reservationInvalid));
        }

        [Fact]
        public async Task CheckAvailabilityIsTrueTest()
        {
            // arrange
            ReservationDto ReservationResult = new ReservationDto();
            Timestamp Von = Timestamp.FromDateTime(new DateTime(2021, 09, 13, 0, 0, 0, DateTimeKind.Utc));
            Timestamp Bis = Timestamp.FromDateTime(new DateTime(2021, 09, 20, 0, 0, 0, DateTimeKind.Utc));
            KundeDto Client = _kundeClient.Get(new KundeRequest { Id = 2 });
            AutoDto Car = _autoClient.Get(new AutoRequest { Id = 2 });

            ReservationResult.Von = Von;
            ReservationResult.Bis = Bis;
            ReservationResult.Kunde = Client;
            ReservationResult.Auto = Car;

            // act
            IsCarAvailableResponse result = _target.IsCarAvailable(ReservationResult);

            // assert
            Assert.True(result.IsAvailable);
        }

        [Fact]
        public async Task CheckAvailabilityIsFalseTest()
        {
            // arrange
            ReservationRequest existingRequest = new ReservationRequest { Id = 1 };
            ReservationDto existingDto = _target.Get(existingRequest);

            ReservationDto reservation = new ReservationDto { Auto = existingDto.Auto, Kunde = existingDto.Kunde, Von = existingDto.Von, Bis = existingDto.Bis };

            // act
            bool availability = _target.IsCarAvailable(reservation).IsAvailable;

            // assert
            Assert.False(availability);
        }
    }
}