using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinalMovil2.Models
{
    public class Usuarios
    {
        public string Id_User { get; set; }
        public string Id_User_Cliente { get; set; }

        public string Nombres { get; set; }

        public string Correo { get; set; }

        public string Contrasena { get; set; }

        public string ImagenPerfil { get; set; }

        public string Tipo_Usuario { get; set; }
    }
}
