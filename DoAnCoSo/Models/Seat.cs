namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Seat")]
    public partial class Seat
    {
        [Key]
        public int SeatId { get; set; }

        public int? SeatNumber { get; set; }

        public int? Availability { get; set; }
    }
}
