namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Message")]
    public partial class Message
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Sender { get; set; }
        [StringLength(255)]
        public string Phone { get; set; }

        public string MessageText { get; set; }
        public string ReplyMessage { get; set; }

        public DateTime? SentTime { get; set; }
    }
}
