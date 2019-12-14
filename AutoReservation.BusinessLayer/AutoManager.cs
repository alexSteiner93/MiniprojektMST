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

        public void DeleteCar(Auto Car)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(Car).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public async Task<Auto> GetCarByPrimary(int primary)
        {
            using AutoReservationContext context = new AutoReservationContext();

            return await context.Autos.SingleAsync(a => a.Id == primary);

        }

        public void UpdateCar(Auto auto)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(auto).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void AddCar(Auto newCar)
        {
            using AutoReservationContext context = new AutoReservationContext();

            context.Autos.AddAsync(newCar);
            context.SaveChanges();

        }
    }
}

    
