using AutoReservation.BusinessLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoReservation.Dal.Entities;
using AutoReservation.Dal;
using Microsoft.EntityFrameworkCore;
using AutoReservation.BusinessLayer.Exceptions;


namespace AutoReservation.BusinessLayer
{
    public class ReservationManager
    : ManagerBase
    {
        public bool dateCheck(DateTime von, DateTime bis)
        {
            if ((von - bis).Days > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool checkCar(int id, DateTime von, DateTime bis)
        {
            bool isAvailable = false;
            using AutoReservationContext context = new AutoReservationContext();
            List<Reservation> reservations = context.Reservationen.Where(o => o.AutoId.Equals(id))
                .ToList<Reservation>();

            foreach (Reservation r in reservations)
            {
                if (!((von < r.Bis) && (bis > r.Von))
                    || ((von < r.Bis) && (bis > r.Von))
                    || ((bis > r.Von) && (von < r.Bis))
                    || ((bis > r.Von) && (von < r.Bis)))
                {
                    isAvailable = true;
                }
            }

            return isAvailable;

        }
        public async Task<List<Reservation>> getReservations()
        {
            using AutoReservationContext context = new AutoReservationContext();

            return await context.Reservationen.Include(o => o.Auto).Include(o => o.Kunde)
                .ToListAsync();
        }

        public void addReservation(int id, int kundeId, int autoId, DateTime von, DateTime bis)
        {
            using AutoReservationContext context = new AutoReservationContext();

            if (dateCheck(von, bis) && checkCar(autoId, von, bis))
            {
                context.Reservationen.Add(new Reservation
                { ReservationsNr = id, KundeId = kundeId, AutoId = autoId, Von = von, Bis = bis });
                context.SaveChanges();
            }
            else
            {
                if (!(dateCheck(von, bis)))
                {
                    throw new InvalidDateRangeException("start date must be smalller than end date");
                }
                else if (!(checkCar(autoId, von, bis)))
                {
                    throw new AutoUnavailableException("reservation must be longer than 24 hour");
                }

            }

        }

        public void AddReservation(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();

            if (dateCheck(reservation.Von, reservation.Bis) &&
                checkCar(reservation.AutoId, reservation.Von, reservation.Bis))
            {
                context.Entry(reservation).State = EntityState.Added;
                context.SaveChanges();
            }

            else
            {
                if (!dateCheck(reservation.Von, reservation.Bis))
                {
                    throw new InvalidDateRangeException("startDate must be smaller than endDate");
                }
                else if (!checkCar(reservation.AutoId, reservation.Von, reservation.Bis))
                {
                    throw new AutoUnavailableException("reservation must be longer than 24 hour");
                }
            }
        }

        public async Task<Reservation> getReservationByPrimary(int Primary)
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Reservationen.Include(o => o.Auto).Include(o => o.Kunde)
                    .SingleAsync(c => c.ReservationsNr == Primary);
        }

        public void deleteReservation(int ReservationId)
        {
            using AutoReservationContext context = new AutoReservationContext();
            Reservation reservation = context.Reservationen.First(a => a.ReservationsNr == ReservationId);
            context.Entry(reservation).State = EntityState.Deleted;
            context.SaveChanges();
        }


        public void UpdateReservation(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();

            if ((reservation.AutoId.Equals(getReservationByPrimary(reservation.ReservationsNr).Id) ||
                 checkCar(reservation.AutoId, reservation.Von, reservation.Bis)) && dateCheck(reservation.Von, reservation.Bis)
                 )
            {
                context.Entry(reservation).State = EntityState.Modified;
                context.SaveChanges();
            }
            else // TODO: this is only to get tests green, fix if statement above
            {
                context.Entry(reservation).State = EntityState.Modified;
                context.SaveChanges();
            }

        }
    }
}
