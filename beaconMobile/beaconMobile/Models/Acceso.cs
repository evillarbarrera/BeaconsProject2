using System;
using System.Collections.Generic;
using System.Text;

namespace beaconMobile.Models
{
    public class Acceso
    {
        public string tipo { get; set; }
        public string direccion { get; set; }
        public double fecha_lectura { get; set; }
        public string nombre_centro { get; set; }
        public string nombre_persona { get; set; }
        public string rut_persona { get; set; }
        public string funcion_persona { get; set; }
        public int id_beacon { get; set; }
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
