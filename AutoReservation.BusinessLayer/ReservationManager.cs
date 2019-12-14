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

        public async Task<List<Reservation>> GetReservations()
        {
            using AutoReservationContext context = new AutoReservationContext();

            return await context.Reservationen.Include(o => o.Auto).Include(o => o.Kunde)
                .ToListAsync();
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

        public async Task<Reservation> GetReservationByPrimary(int Primary)
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Reservationen.Include(o => o.Auto).Include(o => o.Kunde)
                    .SingleAsync(c => c.ReservationsNr == Primary);
        }

        public void DeleteReservation(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(reservation).State = EntityState.Deleted;
            context.SaveChanges();
        }


        public void UpdateReservation(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();

            if (IsCarAvailable(reservation.AutoId, reservation.Von, reservation.Bis) && IsDateCorrect(reservation.Von, reservation.Bis)
                 )
            {
                context.Entry(reservation).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
