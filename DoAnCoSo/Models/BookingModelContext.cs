using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DoAnCoSo.Models
{
    public partial class BookingModelContext : DbContext
    {
        public BookingModelContext()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<Appointments> Appointments { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

        public virtual DbSet<Barbers> Barbers { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Seat> Seats { get; set; }
        public virtual DbSet<Ratings> Ratings { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<Styles> Styles { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
