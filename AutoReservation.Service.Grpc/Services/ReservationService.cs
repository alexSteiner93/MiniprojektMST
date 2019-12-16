
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

using Microsoft.Extensions.Logging;

namespace AutoReservation.Service.Grpc.Services
{
    internal class ReservationService : Grpc.ReservationService.ReservationServiceBase
    {
        private readonly ILogger<ReservationService> _logger;
        private ReservationManager ReservationsManager;

        public ReservationService(ILogger<ReservationService> logger)
        {
            _logger = logger;
            ReservationsManager = new ReservationManager();
        }


        public override async Task<ReservationAllDto> GetReservations(Empty request, ServerCallContext context)
        {

            List<Reservation> Reservations = await ReservationsManager.GetReservations();
            List<ReservationDto> result = Reservations.ConvertToDtos();
            
            return result;
        }

        public override async Task<ReservationDto> Get(ReservationRequest request, ServerCallContext context)
        {
            Reservation result = await ReservationsManager.GetReservationByPrimary(request.Id);
          
            return result.ConvertToDto();
        }

        public override void AddReservation(ReservationDto request, ServerCallContext context)
        { 

            ReservationsManager.AddReservation(request.ConvertToEntity());
               
       
        }

        public override void UpdateReservation(ReservationDto request, ServerCallContext context)
        {
            ReservationsManager.UpdateReservation(request.ConvertToEntity());
        }

        public override void DeleteReservation (ReservationDto request, ServerCallContext context)
        {
            ReservationsManager.DeleteReservation(request.ConvertToEntity());
        }

    }
}
