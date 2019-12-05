using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using System.Linq;
namespace AutoReservation.BusinessLayer

{
    public class AutoManager
        : ManagerBase
    {

        public async Task<List<Auto>> GetAllCars()
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Autos.ToListAsync();
        }

        public void deleteCar(int CarId)
        {
            using AutoReservationContext context = new AutoReservationContext();
            Auto car = context.Autos.First(a => a.Id == CarId);
            context.Entry(car).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public async Task<Auto> getCarByPrimary(int primary)
        {
            using AutoReservationContext context = new AutoReservationContext();

            return await context.Autos.SingleAsync(a => a.Id == primary);

        }

        public void updateCar(Auto auto)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(auto).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}

    
