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
            KundeManager ClientManager = new KundeManager();
            List<KundeDto> temp = await ClientManager.GetAll().ConvertToDtos();
            KundeAllDto result = new KundeAllDto();
            temp.ForEach(kundeDto => result.Clients.Add(kundeDto));
            return result;
        }

        public override async Task<KundeDto> Get(KundeRequest request, ServerCallContext context)
        {
            KundeManager ClientManager = new KundeManager();
            Kunde result = await ClientManager.Get(request.Id);
            if (result == null)
            {
                throw new RpcException(new Status(
                    StatusCode.OutOfRange,
                    "Client not found"
                ));
            }
            return result.ConvertToDto();
        }

        public override async Task<KundeDto> Insert(KundeDto request, ServerCallContext context)
        {
            KundeManager ClientManager = new KundeManager();
            Kunde result = await ClientManager.Insert(request.ConvertToEntity());
            return result.ConvertToDto();
        }

        public override async Task<Empty> Update(KundeDto request, ServerCallContext context)
        {
            KundeManager ClientManager = new KundeManager();
          
            try
            {
                await ClientManager.Update(request.ConvertToEntity());
            }
            catch (OptimisticConcurrencyException<Kunde> exception)
            { 
                throw new RpcException(
                    new Status(StatusCode.Aborted,"Concurrency Exception."),
                    exception.ToString()
                    );
            }
            return new Empty();
        }

        public override async Task<Empty> Delete(KundeDto request, ServerCallContext context)
        {
            KundeManager CLientManager = new KundeManager();
            await CLientManager.Delete(request.ConvertToEntity());
            return new Empty();
        }
    }
}