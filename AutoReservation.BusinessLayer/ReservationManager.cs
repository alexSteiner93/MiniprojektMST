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
        public bool CheckDate(DateTime von, DateTime bis)
        {
            if ((bis - von).TotalHours >= 24) return true;

            return false;
        }
        public async Task<bool> IsCarAvailable(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();
            bool isOverlapping = await context.Reservationen.AnyAsync(existing =>
                (existing.ReservationsNr != reservation.ReservationsNr)
                && (existing.Von < reservation.Bis && reservation.Von < existing.Bis)
                && (existing.AutoId == reservation.AutoId)
                );

            return !isOverlapping;
        }

        public async Task<bool> CheckAvailability (Reservation reservation)
        {
            bool isAvailable = await IsCarAvailable(reservation);
            return CheckDate(reservation.Von, reservation.Bis) && isAvailable;
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

            if (await CheckAvailability(reservation))
            {
                context.Entry(reservation).State = EntityState.Added;
                await context.SaveChangesAsync();
                return reservation;
            }

            else
            {
                if (!CheckDate(reservation.Von, reservation.Bis))
                {
                    throw new InvalidDateRangeException("Invalid Date range");
                }
                else if (!await IsCarAvailable(reservation))
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

            if (await CheckAvailability(reservation))
            {
                context.Entry(reservation).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return reservation;
            } else
            {
                if (!CheckDate(reservation.Von, reservation.Bis))
                {
                    throw new InvalidDateRangeException("Invalid Date range");
                }
                else if (!await IsCarAvailable(reservation))
                {
                    throw new AutoUnavailableException("car not available");
                }
            }
            return null;
        }
    }
}
