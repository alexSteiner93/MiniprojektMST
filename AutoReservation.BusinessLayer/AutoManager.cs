using System;
using System.Collections.Generic;
using System.Linq;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
namespace AutoReservation.BusinessLayer
{
    public class AutoManager
        : ManagerBase
    {

        public async Task<List<Auto>> GetAll()
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Autos.ToListAsync();
        }

        public async Auto getAutoByPrimary(int primary)
        {
            using AutoReservationContext context = new AutoReservationContext();


                Auto auto = context.Autos.Single(a => a.Id == id)

                return auto;

        }

        public async void updateAuto(Auto auto)
        {
            using AutoReservationContext context = new AutoReservationContext();



            context.Entry(updatedAuto).State = EntityState.Modified;
            context.SaveChanges();

        }

        public void DeleteAuto(int id)
        {
            using AutoReservationContext context = new AutoReservationContext();

            Auto auto = context.Autos.First(a => a.Id == id);

            context.Entry(auto).State = EntityState.Deleted;
            context.SaveChanges();

        }

    }
}

    
