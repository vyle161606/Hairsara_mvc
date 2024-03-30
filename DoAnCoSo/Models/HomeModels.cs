using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnCoSo.Models
{
    public class HomeModels
    {
        public List<Services> listServices { get; set; }
        public List<News> listNews { get; set; }
        public IEnumerable<Barbers> BarbersList { get; set; }
        public Barbers NewBarber { get; set; }
    }
}