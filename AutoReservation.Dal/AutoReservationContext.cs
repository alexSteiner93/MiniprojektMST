﻿using System.Configuration;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace AutoReservation.Dal
{
    public class AutoReservationContext
        : AutoReservationContextBase :DbContext
    {

        public DbSet<Auto> Autos { get; set; }

        public DbSet<Kunde> Kunden { get; set; }

        public DbSet<Reservation> Reservationen { get; set; }

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auto>()
            .ToTable("Auto", "dbo")
            .HasDiscriminator<int>("AutoKlasse")
            .HasValue<LuxusklasseAuto>(0)
            .HasValue<MittelklasseAuto>(1)
            .HasValue<StandardAuto>(2)
            ;

        modelBuilder.Entity<Kunde>().ToTable("Kunde", schema: "dbo");
        modelBuilder.Entity<Reservation>().ToTable("Reservation", schema: "dbo");
    }
}
