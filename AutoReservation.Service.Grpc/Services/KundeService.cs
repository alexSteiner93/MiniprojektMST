using System.Collections.Generic;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace AutoReservation.Service.Grpc.Services
{
    internal class KundeService : Grpc.KundeService.KundeServiceBase
    {
        private readonly ILogger<KundeService> _logger;

        public KundeService(ILogger<KundeService> logger)
        {
            _logger = logger;
        }

        public override async Task<KundeAllDto> GetAll(Empty request, ServerCallContext context)
        {
            KundeManager manager = new KundeManager();
            List<Kunde> allCustomers = await manager.GetAll();
            List<KundeDto> customerDtos = allCustomers.ConvertToDtos();
            KundeAllDto customersDto = new KundeAllDto();
            customerDtos.ForEach(kundeDto => customersDto.Clients.Add(kundeDto));
            return customersDto;
        }

        public override async Task<KundeDto> Get(KundeRequest request, ServerCallContext context)
        {
            KundeManager manager = new KundeManager();
            Kunde customer = await manager.Get(request.Id);
            if (customer == null)
            {
                throw new RpcException(new Status(
                    StatusCode.OutOfRange, "Id couldn't be found."
                ));
            }
            return customer.ConvertToDto();
        }

        public override async Task<KundeDto> Insert(KundeDto request, ServerCallContext context)
        {
            KundeManager manager = new KundeManager();
            Kunde kunde = request.ConvertToEntity();
            Kunde newKunde = await manager.Insert(kunde);
            return newKunde.ConvertToDto();
        }

        public override async Task<Empty> Update(KundeDto request, ServerCallContext context)
        {
            KundeManager manager = new KundeManager();
            Kunde kunde = request.ConvertToEntity();
            try
            {
                await manager.Update(kunde);
            }
            catch (OptimisticConcurrencyException<Kunde> exception)
            { 
                throw new RpcException(new Status(
                    StatusCode.Aborted,
                    "Conccurency Exception."
                ), exception.ToString());
            }
            return new Empty();
        }

        public override async Task<Empty> Delete(KundeDto request, ServerCallContext context)
        {
            KundeManager manager = new KundeManager();
            Kunde kunde = request.ConvertToEntity();
            await manager.Delete(kunde);
            return new Empty();
        }
    }
}