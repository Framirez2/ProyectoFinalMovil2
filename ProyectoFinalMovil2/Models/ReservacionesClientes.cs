using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinalMovil2.Models
{
    public class ReservacionesClientes
    {
        public string Id_Reservaciones { get; set; }

        public string Id_Cliente { get; set; }

        public string Nombre_Usuario { get; set; }

        public string Nombre_Estilisita { get; set; }

        public string Hora_Reservacion { get; set; }

        public string Fecha_Reservacion { get; set; }

        public string Tipo_Reservacion { get; set; }

        public string Precio { get; set; }

        public string Estado { get; set; }

        public int Calificacion { get; set; }
    }
}
