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
        public bool IsCarAvailable(Reservation reservation)
        {
            bool isAvailable = false;
            using AutoReservationContext context = new AutoReservationContext();
            List<Reservation> reservations = context.Reservationen.Where(o => o.AutoId.Equals(reservation.AutoId)).ToList();

            if (reservations.Count == 0) return true;

            foreach (Reservation existing in reservations)
            {
                if ((reservation.Von > existing.Bis) // after
                    || (reservation.Bis < existing.Von) // before
                    || (reservation.ReservationsNr == existing.ReservationsNr))
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

        public bool IsAvailable (Reservation reservation)
        {
            return IsDateCorrect(reservation.Von, reservation.Bis) && IsCarAvailable(reservation);
        }

        public async Task<List<Reservation>> GetAll()
        {
            using AutoReservationContext context = new AutoReservationContext();

            return await context.Reservationen.Include(o => o.Auto).Include(o => o.Kunde)
                .ToListAsync();
        }

        public async Task<Reservation> Insert(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();

            if (IsAvailable(reservation))
            {
                context.Entry(reservation).State = EntityState.Added;
                await context.SaveChangesAsync();
                return reservation;
            }

            else
            {
                if (!IsDateCorrect(reservation.Von, reservation.Bis))
                {
                    throw new InvalidDateRangeException("Invalid Date range");
                }
                else if (!IsCarAvailable(reservation))
                {
                    throw new AutoUnavailableException("car not available");
                }

                return null;
            }
        }

        public async Task<Reservation> Get(int Primary)
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Reservationen.Include(o => o.Auto).Include(o => o.Kunde)
                    .SingleAsync(c => c.ReservationsNr == Primary);
        }

        public async Task<Reservation> Delete(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(reservation).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return reservation;
        }


        public async Task<Reservation> Update(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();

            if (IsAvailable(reservation) && IsDateCorrect(reservation.Von, reservation.Bis))
            {
                context.Entry(reservation).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return reservation;
            } else
            {
                if (!IsDateCorrect(reservation.Von, reservation.Bis))
                {
                    throw new InvalidDateRangeException("Invalid Date range");
                }
                else if (!IsCarAvailable(reservation))
                {
                    throw new AutoUnavailableException("car not available");
                }
            }
            return null;
        }
    }
}
