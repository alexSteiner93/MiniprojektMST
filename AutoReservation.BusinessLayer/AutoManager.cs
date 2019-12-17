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

        public async Task<List<Auto>> GetAll()
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Autos.ToListAsync();
        }

        public async Task<Auto> Get(int primary)
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Autos.FindAsync(primary);
        }

        public async Task<Auto> Insert(Auto car)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(car).State = EntityState.Added;
            await context.SaveChangesAsync();
            return car;
        }

        public async Task Update(Auto car)
        {
            using AutoReservationContext context = new AutoReservationContext();
            try
            {
                context.Entry(car).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw CreateOptimisticConcurrencyException(context, car);
            }
        }

        public async Task<Auto> Delete(Auto car)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(car).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return car;
        }
    }
}

    
