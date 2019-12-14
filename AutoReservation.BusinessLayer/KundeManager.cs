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
        public async Task<List<Kunde>> GetClients()
        {
            using AutoReservationContext context = new AutoReservationContext();
         
               return await context.Kunden.ToListAsync();
            
        }

        public async Task<Kunde> GetCientById(int ClientId)
        {
            using AutoReservationContext context = new AutoReservationContext();

            return await context.Kunden.SingleAsync(c => c.Id == ClientId);
        }

        public void UpdateClient(Kunde Client)
        {
            using AutoReservationContext context = new AutoReservationContext();
                context.Entry(Client).State = EntityState.Modified;
                context.SaveChanges();
            
        }

        public void DeleteClient(Kunde Client)
        {
            using AutoReservationContext context = new AutoReservationContext();
         
                context.Kunden.Remove(Client);
                context.SaveChanges();
            
        }


        public void AddClient(Kunde newClient)
        {
            using AutoReservationContext context = new AutoReservationContext();

            context.Kunden.AddAsync(newClient);
            context.SaveChanges();

        }
    }
}