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
    internal class AutoService : Grpc.AutoService.AutoServiceBase
    {
        private readonly ILogger<AutoService> _logger;
        private AutoManager CarManager;

        public AutoService(ILogger<AutoService> logger)
        {
            _logger = logger;
            CarManager = new AutoManager();
        }

        public override async Task<AutoDto> Get(AutoRequest request, ServerCallContext context)
        {
            Auto result = await CarManager.Get(request.Id);
            if (result == null)
            {
                throw new RpcException(
                    new Status(StatusCode.OutOfRange, 
                    "Car not found"
                ));
            }
            return result.ConvertToDto();
        }

        public override async Task<Empty> Update(AutoDto request, ServerCallContext context)
        {
            try
            {
                await CarManager.Update(request.ConvertToEntity());
            }
            catch (OptimisticConcurrencyException<Auto> exception)
            {
                throw new RpcException(
                    new Status(StatusCode.Aborted,"Concurrency exception"), 
                    exception.ToString()
                );
            }
            return new Empty();
        }

        public override async Task<Empty> Delete(AutoDto request, ServerCallContext context)
        {
            await CarManager.Delete(request.ConvertToEntity());
            return new Empty();
        }

        public override async Task<AutoAllDto> GetAll(Empty request, ServerCallContext context)
        {

            List<AutoDto> dtos = await CarManager.GetAll().ConvertToDtos();
            AutoAllDto result = new AutoAllDto();
            dtos.ForEach(autoDto => result.Cars.Add(autoDto));
            return result;
        }

        public override async Task<AutoDto> Insert(AutoDto request, ServerCallContext context)
        {
            Auto result = await CarManager.Insert(request.ConvertToEntity());
            return result.ConvertToDto();
        }
    }
}