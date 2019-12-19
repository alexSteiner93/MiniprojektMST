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
        private ReservationManager _reservationManager;

        public ReservationService(ILogger<ReservationService> logger)
        {
            _logger = logger;
            _reservationManager = new ReservationManager();
        }

        public override async Task<ReservationDto> Get(ReservationRequest request, ServerCallContext context)
        {
          
            return await _reservationManager.Get(request.Id).ConvertToDto();
        }

        public override async Task<ReservationDto> Insert(ReservationDto request, ServerCallContext context)
        { 
            try
            {
                Reservation newreservation = await _reservationManager.Insert(request.ConvertToEntity());
                return newreservation.ConvertToDto();
            }
            catch (InvalidDateRangeException exception)
            {
                throw new RpcException(
                    new Status(StatusCode.FailedPrecondition, "Reservation must be at least 24h"), 
                    exception.ToString()
                );
            }
            catch (AutoUnavailableException exception)
            {
                throw new RpcException(
                    new Status(StatusCode.FailedPrecondition, "Car is not available"), 
                    exception.ToString()
                );
            }
        }

        public override async Task<IsCarAvailableResponse> IsCarAvailable(ReservationDto request, ServerCallContext context)
        {
            Reservation result = request.ConvertToEntity();
            return new IsCarAvailableResponse { IsAvailable = _reservationManager.IsCarAvailable(result) };
        }

        public override async Task<Empty> Update (ReservationDto request, ServerCallContext context)
        {
            try
            {
                await _reservationManager.Update(request.ConvertToEntity());
            }
            catch (OptimisticConcurrencyException<Reservation> exception)
            {
                throw new RpcException(
                    new Status(StatusCode.Aborted, "Concurrency exception"), 
                    exception.ToString()
                );
            }
            catch (InvalidDateRangeException exception)
            {
                throw new RpcException(
                    new Status(StatusCode.FailedPrecondition, "Reservation must be at least 24h"), 
                    exception.ToString()
                );
            }
            catch (AutoUnavailableException exception)
            {
                throw new RpcException(
                    new Status(StatusCode.FailedPrecondition, "Car is not available"), 
                    exception.ToString()
                );
            }
            return new Empty();
        }

        public override async Task<ReservationAllDto> GetAll(Empty request, ServerCallContext context)
        {

            List<ReservationDto> temp = await _reservationManager.GetAll().ConvertToDtos();
            ReservationAllDto result = new ReservationAllDto();
            foreach (ReservationDto reservationDto in temp)
            {
                result.Reservations.Add(reservationDto);
            }

            return result;
        }

        public override async Task<Empty> Delete (ReservationDto request, ServerCallContext context)
        {
            await _reservationManager.Delete(request.ConvertToEntity());
            return new Empty();
        }
    }
}
