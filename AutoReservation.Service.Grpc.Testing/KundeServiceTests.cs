using System;
using System.Threading.Tasks;
using AutoReservation.Service.Grpc.Testing.Common;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Xunit;

namespace AutoReservation.Service.Grpc.Testing
{
    public class KundeServiceTests
        : ServiceTestBase
    {
        private readonly KundeService.KundeServiceClient _target;

        public KundeServiceTests(ServiceTestFixture serviceTestFixture)
            : base(serviceTestFixture)
        {
            _target = new KundeService.KundeServiceClient(Channel);
        }

        [Fact]
        public async Task GetKundenTest()
        {
            KundeAllDto result = _target.GetAll(new Empty());
            RepeatedField<KundeDto> kundeDtos = result.Clients;
            Assert.Equal(4, kundeDtos.Count);
        }

        [Fact]
        public async Task GetKundeByIdTest()
        {
            KundeDto result = _target.Get(new KundeRequest { Id = 1 });

            Assert.Equal("Anna", result.Vorname);

        }

        [Fact]
        public async Task GetKundeByIdWithIllegalIdTest()
        {
            // arrange
            KundeRequest toGet = new KundeRequest { Id = -1 };

            // assert
            Assert.Throws<RpcException>(() => _target.Get(toGet));
        }

        [Fact]
        public async Task InsertKundeTest()
        {
            // arrange
            KundeDto kunde = new KundeDto { Nachname = "Trump", Vorname = "Donald" , Geburtsdatum = new DateTime(1946, 6, 14).ToUniversalTime().ToTimestamp()};

            // act
            KundeDto newKunde = _target.Insert(kunde);
            KundeDto actual = _target.Get(new KundeRequest { Id = newKunde.Id });
            
            // assert
            Assert.Equal("Trump", actual.Nachname);
        }

        [Fact]
        public async Task DeleteKundeTest()
        {
            // arrange
            KundeRequest toDelete = new KundeRequest { Id = 1 };
            KundeDto kunde = _target.Get(toDelete);

            // act
            _target.Delete(kunde);

            // assert
            Assert.Throws<RpcException>(() => _target.Get(toDelete));
        }

        [Fact]
        public async Task UpdateKundeTest()
        {
            // arrange
            string newNachname = "asdf";
            KundeRequest toUpdate = new KundeRequest { Id = 1 };
            KundeDto kunde = _target.Get(toUpdate);

            // act
            kunde.Nachname = newNachname;
            _target.Update(kunde);

            // assert
            Assert.Equal(newNachname, _target.Get(toUpdate).Nachname);
        }

        [Fact]
        public async Task UpdateKundeWithOptimisticConcurrencyTest()
        {
            // arrange
            KundeRequest toUpdate = new KundeRequest { Id = 1 };
            KundeDto kunde = _target.Get(toUpdate);
            KundeDto concurrentKunde = _target.Get(toUpdate);

            // act
            _target.Update(concurrentKunde);

            // assert
            Assert.Throws<RpcException>(() => _target.Update(kunde));
        }
    }
}