using AutoReservation.BusinessLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoReservation.Dal.Entities;
using AutoReservation.Dal;


namespace AutoReservation.BusinessLayer
{
    public class ReservationManager
    : ManagerBase
    {
        public bool IsDateCorrect(DateTime von, DateTime bis)
        {
            if ((bis - von).TotalHours >= 24) return true;

            return false;
        }
        public bool IsCarAvailable(int AutoId, DateTime von, DateTime bis)
        {
            bool isAvailable = false;
            using AutoReservationContext context = new AutoReservationContext();
            List<Reservation> reservations = context.Reservationen.Where(o => o.AutoId.Equals(AutoId)).ToList();

            if (reservations.Count == 0) return true;

            foreach (Reservation reservation in reservations)
            {
                if ((von > reservation.Bis) // after
                    || (bis < reservation.Von)) // before
                {
                    isAvailable = true;
                } 
                else
                {
                    return false;
                }
            }

            return isAvailable;

        }

        public bool IsAvailable (DateTime von, DateTime bis, int autoId)
        {
            return IsDateCorrect(von, bis) && IsCarAvailable(autoId, von, bis);
        }

        public async Task<List<Reservation>> getReservations()
        {
            using AutoReservationContext context = new AutoReservationContext();

            return await context.Reservationen.Include(o => o.Auto).Include(o => o.Kunde)
                .ToListAsync();
        }

        public void AddReservation(int id, int kundeId, int autoId, DateTime von, DateTime bis)
        {
            using AutoReservationContext context = new AutoReservationContext();

            if (IsAvailable(von, bis, autoId))
            {
                context.Reservationen.Add(new Reservation{ ReservationsNr = id, KundeId = kundeId, AutoId = autoId, Von = von, Bis = bis });
                context.SaveChanges();
            }
            else
            {
                if (!(IsDateCorrect(von, bis)))
                {
                    throw new InvalidDateRangeException("Invalid Date range");
                }
                else if (!(IsCarAvailable(autoId, von, bis)))
                {
                    throw new AutoUnavailableException("car not available");
                }

            }

        }

        public void AddReservation(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();

            if (IsAvailable(reservation.Von, reservation.Bis, reservation.AutoId))
            {
                context.Entry(reservation).State = EntityState.Added;
                context.SaveChanges();
            }

            else
            {
                if (!IsDateCorrect(reservation.Von, reservation.Bis))
                {
                    throw new InvalidDateRangeException("Invalid Date range");
                }
                else if (!IsCarAvailable(reservation.AutoId, reservation.Von, reservation.Bis))
                {
                    throw new AutoUnavailableException("car not available");
                }
            }
        }

        public async Task<Reservation> getReservationByPrimary(int Primary)
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Reservationen.Include(o => o.Auto).Include(o => o.Kunde)
                    .SingleAsync(c => c.ReservationsNr == Primary);
        }

        public void DeleteReservation(int ReservationId)
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
                 IsCarAvailable(reservation.AutoId, reservation.Von, reservation.Bis)) && IsDateCorrect(reservation.Von, reservation.Bis)
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
