using AutoReservation.BusinessLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoReservation.Dal.Entities;
using AutoReservation.Dal;
using Microsoft.EntityFrameworkCore;


namespace AutoReservation.BusinessLayer
{
    public class ReservationManager
    : ManagerBase
    {
        public async Task<List<Reservation>> getReservations()
        {
            using AutoReservationContext context = new AutoReservationContext();

            return await context.Reservationen.Include(o => o.Auto).Include(o => o.Kunde)
                .ToListAsync();
        }

        public void addReservation(int id, int kundeId, int autoId, DateTime von, DateTime bis)
        {
            using AutoReservationContext context = new AutoReservationContext();

            if (DateRangeCheck(von, bis) && IsCarAvailable(autoId, von, bis))
            {
                context.Reservationen.Add(new Reservation
                { ReservationsNr = id, KundeId = kundeId, AutoId = autoId, Von = von, Bis = bis });
                context.SaveChanges();
            }

        }

        public async Task<Reservation> getReservationByPrimary(int Primary)
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Reservationen.Include(o => o.Auto).Include(o => o.Kunde)
                    .SingleAsync(c => c.ReservationsNr == Primary);
        }

        public bool DateRangeCheck(DateTime? von, DateTime? bis)
        {
            TimeSpan diff = bis.Subtract(von);
            if (bis > von && diff.Days >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddReservation(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();

            if (DateRangeCheck(reservation.Von, reservation.Bis) &&
                IsCarAvailable(reservation.AutoId, reservation.Von, reservation.Bis))
            {
                context.Entry(reservation).State = EntityState.Added;
                context.SaveChanges();
            }
          

        }

        public void UpdateReservation(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext();

            if ((reservation.AutoId.Equals(GetReservationById(reservation.ReservationsNr).AutoId) ||
                 IsCarAvailable(reservation.AutoId, reservation.Von, reservation.Bis)) && DateRangeCheck(reservation.Von, reservation.Bis)
                 )
            {
                context.Entry(reservation).State = EntityState.Modified;
                context.SaveChanges();
            }

        }

        public void DeleteReservation(int id)
        {
            using AutoReservationContext context = new AutoReservationContext();

            Reservation reservationToBeDeleted = context
                .Reservationen
                .First(a => a.ReservationsNr == id);

            context.Entry(reservationToBeDeleted).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public bool IsCarAvailable(int id, DateTime? von, DateTime? bis)
        {
            bool isAvailable = true;
            using AutoReservationContext context = new AutoReservationContext();
            var reservations = context
                .Reservationen
                .Where(o => o.AutoId.Equals(id))
                .ToList<Reservation>();

            foreach (Reservation r in reservations)
            {
                if (((von < r.Bis) && (bis > r.Von))
                    || ((von < r.Bis) && (bis > r.Von))
                    || ((bis > r.Von) && (von < r.Bis))
                    || ((bis > r.Von) && (von < r.Bis)))
                {
                    isAvailable = false;
                }
            }

            return isAvailable;

        }
    }
}
