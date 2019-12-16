using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace AutoReservation.BusinessLayer
{
    public class KundeManager
        : ManagerBase
    {
        public async Task<List<Kunde>> GetAll()
        {
            using AutoReservationContext context = new AutoReservationContext();
         
               return await context.Kunden.ToListAsync();
        }

        public async Task<Kunde> Get(int clientId)
        {
            using AutoReservationContext context = new AutoReservationContext();

            return await context.Kunden.SingleAsync(c => c.Id == clientId);
        }

        public async Task<Kunde> Insert(Kunde client)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(client).State = EntityState.Added;
            await context.SaveChangesAsync();
            return client;
        }

        public async Task Update(Kunde client)
        {
            using AutoReservationContext context = new AutoReservationContext();
            try
            {
                context.Entry(client).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw CreateOptimisticConcurrencyException(context, client);
            }

        }

        public async Task<Kunde> Delete(Kunde client)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(client).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return client;
        }
    }
}