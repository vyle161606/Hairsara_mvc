namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ratings
    {
        [Key]
        public int RateId { get; set; }

        public int? AppointmentId { get; set; }

        public int? RateScore { get; set; }

        [StringLength(256)]
        public string Comment { get; set; }

        public DateTime? CreateDate { get; set; }

        public virtual Appointments Appointments { get; set; }
    }
}
