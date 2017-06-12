using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rabun.Oanda.Rest.Models
{
    public class OrderOpen
    {
        public string Instrument { get; set; }
        public DateTime Time { get; set; }
        public float Price { get; set; }

    }
}
