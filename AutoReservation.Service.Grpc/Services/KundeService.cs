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
        private KundeManager clientManager;

        public KundeService(ILogger<KundeService> logger)
        {
            _logger = logger;
            clientManager = new KundeManager();

        }


        public override async Task<KundeAllDto> GetClients(Empty request, ServerCallContext context)
        {

            List<Kunde> Clients = await clientManager.GetClients(); 
            List<KundeDto> result = Clients.ConvertToDtos();
            
            return result;
        }

        public override async Task<KundeDto> Get(KundeRequest request, ServerCallContext context)
        {
           
            Kunde client = await clientManager.GetCientById(request.Id);
          
            return client.ConvertToDto();
        }

        public override void AddClient(KundeDto request, ServerCallContext context)
        {
            
            Kunde client = request.ConvertToEntity();
           clientManager.AddClient(client);
            
        }

        public override void UpdateClient(KundeDto request, ServerCallContext context)
        {
           
            Kunde client = request.ConvertToEntity();
            clientManager.UpdateClient(client);
        }

        public override void Delete(KundeDto request, ServerCallContext context)
        {
            
            Kunde client = request.ConvertToEntity();
            clientManager.DeleteClient(client);
            
        }
    }
}
