﻿using System;
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
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task InsertKundeTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task DeleteKundeTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task UpdateKundeTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task UpdateKundeWithOptimisticConcurrencyTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }
    }
}