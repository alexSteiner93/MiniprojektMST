using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AutoReservation.Dal.Entities
{
    public class Kunde
    {

        [Key, Column("KundenId")]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Vorname { get; set; }
        [MaxLength(20)]
        public string Nachname { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime Geburtsdatum { get; set; }

        public byte[] RowVersion { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

    }

}