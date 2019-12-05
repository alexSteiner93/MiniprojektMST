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
        public async Task<List<Kunde>> getClients()
        {
            using AutoReservationContext context = new AutoReservationContext();
         
               return await context.Kunden.ToListAsync();
            
        }

        public void addClient(String name, String surname, DateTime birthday)
        {
        using AutoReservationContext context = new AutoReservationContext();
            
                context.Kunden.AddAsync(new Kunde { Nachname = name, Vorname = surname, Geburtsdatum = birthday });
                context.SaveChanges();
            
        }

        public async Task<Kunde> getCLientsById(int ClientId)
        {
            using AutoReservationContext context = new AutoReservationContext();

            return await context.Kunden.SingleAsync(c => c.Id == ClientId);
        }

        public void updateClient(Kunde Client)
        {
            using AutoReservationContext context = new AutoReservationContext();
                context.Entry(Client).State = EntityState.Modified;
                context.SaveChanges();
            
        }

        public void deleteClient(int ClientId)
        {
            using AutoReservationContext context = new AutoReservationContext();
            
                Kunde client = context.Kunden.Single(c => c.Id == ClientId);
                context.Kunden.Remove(client);
                context.SaveChanges();
            
        }


        public void addClient(Kunde newClient)
        {
            using AutoReservationContext context = new AutoReservationContext();

            context.Kunden.AddAsync(newClient);
            context.SaveChanges();

        }
    }
}