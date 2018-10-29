using System;
using System.Collections.Generic;
using System.Text;

namespace beaconMobile.Models
{
    public class Beacon
    {
        public string ui_beacon { get; set; }
        public int mayor { get; set; }
        public int minor { get; set; }
        public double fecha_lectura { get; set; }
        public DateTime fecha_lecturaDatetime
        {

            get
            {
                var unix = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return unix.AddSeconds(this.fecha_lectura);
            }
            set
            {
                var unix = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                this.fecha_lectura = Convert.ToInt64((value - unix).TotalSeconds);
            }
        }

    }
}
