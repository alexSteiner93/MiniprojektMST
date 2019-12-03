using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AutoReservation.BusinessLayer
{
    public class KundeManager
        : ManagerBase
    {
        public async List<Kunde> GetKunden()
        {
            using AutoReservationContext context = new AutoReservationContext();
          
                var kunden = context.Kunden;
                return kunden.ToListAsync();
            
        }

        public Kunde GetKundeById(int id)
        {
        using AutoReservationContext context = new AutoReservationContext();
           
                Kunde kunde = context
                    .Kunden
                    .Single(c => c.Id == id);

            return kunde;
        }

        public void AddKunde(String Nachname, String Vorname, DateTime Geburtsdatum)
        {
        using AutoReservationContext context = new AutoReservationContext();
            
                context.Kunden.Add(new Kunde { Nachname = Nachname, Vorname = Vorname, Geburtsdatum = Geburtsdatum });
                context.SaveChanges();
            
        }

        public void AddKunde(Kunde newKunde)
        {
        using AutoReservationContext context = new AutoReservationContext();
            
                context.Kunden.Add(newKunde);
                context.SaveChanges();
            
        }

        public void UpdateKunde(Kunde updatedKunde)
        {
            using AutoReservationContext context = new AutoReservationContext();
            
                context.Entry(updatedKunde).State = EntityState.Modified;
                context.SaveChanges();
            
        }

        public void deleteKunde(int id)
        {
            using AutoReservationContext context = new AutoReservationContext();
            
                Kunde kunde = context
                    .Kunden
                    .Single(c => c.Id == id);

                context.Kunden.Remove(kunde);
                context.SaveChanges();
            
        }
    }
}