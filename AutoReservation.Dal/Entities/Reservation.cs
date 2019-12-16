
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    public class Reservation
    {
        [Key]
        public int ReservationsNr { get; set; }
        
        public int AutoId { get; set; }
        
        public int KundeId { get; set; }
       
        [Column(TypeName = "DATETIME")]
        public DateTime Von { get; set; }
       
        [Column(TypeName = "DATETIME")]
        public DateTime Bis { get; set; }

        [ForeignKey(nameof(KundeId))]
        public Kunde Kunde { get; set; }
        
        [ForeignKey(nameof(AutoId))]
        public Auto Auto { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
