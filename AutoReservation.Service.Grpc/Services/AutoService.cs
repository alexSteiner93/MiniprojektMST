
using AutoReservation.Dal.Entities;
using Google.Protobuf.WellKnownTypes;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer;
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

            CarManager= new AutoManager();
        }

        public override async Task<AutoDto> GetCarByPrimary(AutoRequest request, ServerCallContext context)
        {
            Auto result = await CarManager.GetCarByPrimary(request.Id);
       
            return result.ConvertToDto();
        }

        public override async Task<AutosDto> GetAllCars(Empty request, ServerCallContext context)
        {
            List<Auto> allCars = await CarManager.GetAllCars();
            List<AutoDto> result = allCars.ConvertToDtos();
            return result;
        }

        public override void AddCar (AutoDto request, ServerCallContext context)
        { 
            Auto result = request.ConvertToEntity();
            CarManager.AddCar(result);
            
        }

        public override void UpdateCar(AutoDto request, ServerCallContext context)
        {

            Auto result = request.ConvertToEntity();
            CarManager.UpdateCar(result);


        }
           
      
        public override void DeleteCar(AutoDto request, ServerCallContext context)
        {
          
            CarManager.DeleteCar(request.ConvertToEntity());
            
        }


    }
}
