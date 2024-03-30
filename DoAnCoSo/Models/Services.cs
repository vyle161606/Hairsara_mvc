namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Services
    {
        [Key]
        public int ServiceId { get; set; }

        [StringLength(256)]
        public string ServiceName { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public decimal? Price { get; set; }
    }
}
