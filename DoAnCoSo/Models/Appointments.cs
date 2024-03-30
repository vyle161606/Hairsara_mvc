namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc.Html;

    public partial class Appointments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Appointments()
        {
            Ratings = new HashSet<Ratings>();
        }

        [Key]
        public int AppointmentId { get; set; }
        [Column("ServiceId")]
        public int? ServiceId { get; set; }
        [Column("BarberId")]
        public int? BarberId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        public DateTime? StartDate { get; set; }

        public TimeSpan? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
        [DefaultValue(1)]
        public int? Status { get; set; }

        [StringLength(256)]
        public string Note { get; set; }

        public virtual Barbers Barbers { get; set; }

        public virtual Services Services { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ratings> Ratings { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        public int? SeatId { get; set; }
        public virtual Seat Seat { get; set; }
    }
}
