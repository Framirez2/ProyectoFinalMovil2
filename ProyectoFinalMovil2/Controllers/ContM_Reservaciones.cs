using Firebase.Database.Query;
using ProyectoFinalMovil2.Models;
using ProyectoFinalMovil2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalMovil2.Controllers
{
    public class ContM_Reservaciones
    {
        List<ReservacionesClientes> Reservaciones = new List<ReservacionesClientes>();

        public async Task<string> InsertarReservacion(ReservacionesClientes Reservacion)
        {
            var Dato = await FirebaseConnection.conexionFirebase
                  .Child("Reservaciones")
                  .PostAsync(new ReservacionesClientes()

                  {
                      Id_Cliente = Reservacion.Id_Cliente,
                      Nombre_Estilisita = Reservacion.Nombre_Estilisita,
                      Nombre_Usuario = Reservacion.Nombre_Usuario,
                      Fecha_Reservacion = Reservacion.Fecha_Reservacion,
                      Hora_Reservacion = Reservacion.Hora_Reservacion,
                      Tipo_Reservacion = Reservacion.Tipo_Reservacion,
                      Precio = Reservacion.Precio,
                      Estado = Reservacion.Estado,
                      Calificacion = Reservacion.Calificacion
                  });

            return Dato.Key.ToString();
        }

        public async Task<List<ReservacionesClientes>> GetDateReserv(string Fecha, string Estado)
        {
            List<ReservacionesClientes> ObReservaciones = new List<ReservacionesClientes>();
            var data = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Object.Fecha_Reservacion == Fecha)
                .Where(b => b.Object.Estado == Estado);

            foreach (var pd in data)
            {
                ReservacionesClientes pila = new ReservacionesClientes();
                pila.Id_Reservaciones = pd.Key;
                pila.Nombre_Estilisita = pd.Object.Nombre_Estilisita;
                pila.Precio = pd.Object.Precio;
                pila.Fecha_Reservacion = pd.Object.Fecha_Reservacion;
                pila.Nombre_Usuario = pd.Object.Nombre_Usuario;
                pila.Hora_Reservacion = pd.Object.Hora_Reservacion;
                pila.Estado = pd.Object.Estado;
                pila.Tipo_Reservacion = pd.Object.Tipo_Reservacion;
                pila.Calificacion = pd.Object.Calificacion;

                ObReservaciones.Add(pila);
            }
            return ObReservaciones;
        }

        public async Task<List<ReservacionesClientes>> ObtenerReservacionesFinalizadas()
        {
            List<ReservacionesClientes> listReservaciones = new List<ReservacionesClientes> ();
            var datos = (await FirebaseConnection.conexionFirebase.Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Object.Estado == "Finalizada");

            foreach (var pd in datos)
            {
                ReservacionesClientes pila = new ReservacionesClientes ();
                pila.Id_Reservaciones = pd.Key;
                pila.Nombre_Estilisita = pd.Object.Nombre_Estilisita;
                pila.Nombre_Usuario = pd.Object.Nombre_Usuario;
                pila.Fecha_Reservacion = pd.Object.Fecha_Reservacion;
                pila.Hora_Reservacion = pd.Object.Hora_Reservacion;
                pila.Estado = pd.Object.Estado;
                pila.Precio = pd.Object.Precio;
                pila.Tipo_Reservacion = pd.Object.Tipo_Reservacion;
                pila.Calificacion = pd.Object.Calificacion;

                Reservaciones.Add(pila);
            }

            return Reservaciones;
        }

        public async Task<List<ReservacionesClientes>> ObtenerReservacionesSinFinalizar()
        {
            List<ReservacionesClientes> listReservaciones = new List<ReservacionesClientes> ();

            var datos = (await FirebaseConnection.conexionFirebase.Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Object.Estado == "EnEspera");

            foreach (var pd in datos)
            {
                ReservacionesClientes pila = new ReservacionesClientes();
                pila.Id_Reservaciones = pd.Key;
                pila.Nombre_Estilisita = pd.Object.Nombre_Estilisita;
                pila.Nombre_Usuario = pd.Object.Nombre_Usuario;
                pila.Fecha_Reservacion = pd.Object.Fecha_Reservacion;
                pila.Hora_Reservacion = pd.Object.Hora_Reservacion;
                pila.Estado = pd.Object.Estado;
                pila.Precio = pd.Object.Precio;
                pila.Tipo_Reservacion = pd.Object.Tipo_Reservacion;
                pila.Calificacion = pd.Object.Calificacion;

                Reservaciones.Add(pila);
            }
            return Reservaciones;   
        }

        public async Task<List<ReservacionesClientes>> ObtenerReservaciones(ReservacionesClientes Param)
        {
            List<ReservacionesClientes> ListReservaciones = new List<ReservacionesClientes>();

            var Datos = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Object.Fecha_Reservacion == Param.Fecha_Reservacion);

            foreach (var pd in Datos)
            {
                ReservacionesClientes pila = new ReservacionesClientes();
                pila.Id_Reservaciones = pd.Key;
                pila.Nombre_Estilisita = pd.Object.Nombre_Estilisita;
                pila.Nombre_Usuario = pd.Object.Nombre_Usuario;
                pila.Fecha_Reservacion = pd.Object.Fecha_Reservacion;
                pila.Hora_Reservacion = pd.Object.Hora_Reservacion;
                pila.Estado = pd.Object.Estado;
                pila.Precio = pd.Object.Precio;
                pila.Tipo_Reservacion = pd.Object.Tipo_Reservacion;
                pila.Calificacion = pd.Object.Calificacion;

                Reservaciones.Add(pila);
            }
            return Reservaciones;
        }

        public async Task<List<ReservacionesClientes>> ObtenerReservacionesEstilista(ReservacionesClientes IdCliente)
        {
            List<ReservacionesClientes> Lista2 = new List<ReservacionesClientes>();

            var Datos = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<ReservacionesClientes>())
                .Where(a => a.Object.Fecha_Reservacion == IdCliente.Fecha_Reservacion)
                .Where(a => a.Object.Nombre_Estilisita == IdCliente.Nombre_Estilisita);
            foreach (var pd in Datos)
            {
                var Lista_Horarios = new ReservacionesClientes();
                Lista_Horarios.Hora_Reservacion = pd.Object.Hora_Reservacion;

                Lista2.Add(Lista_Horarios);
            }
            return Lista2;
        }

        public async Task<List<ReservacionesClientes>> GetListB(ReservacionesClientes Obtener_Horas, string NombreEstilista)
        {
            var Datos = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Object.Fecha_Reservacion == Obtener_Horas.Fecha_Reservacion)
                .Where(a => a.Object.Nombre_Estilisita == Obtener_Horas.Nombre_Estilisita);
            foreach (var pd in Datos)
            {
                Obtener_Horas.Id_Cliente = pd.Object.Id_Cliente;
                Obtener_Horas.Hora_Reservacion = pd.Object.Hora_Reservacion;
                Obtener_Horas.Precio = pd.Object.Precio;
                Obtener_Horas.Fecha_Reservacion = pd.Object.Fecha_Reservacion;
                Obtener_Horas.Nombre_Estilisita = pd.Object.Nombre_Estilisita;
                Obtener_Horas.Estado = pd.Object.Estado;

                Reservaciones.Add(Obtener_Horas);
            }
            return Reservaciones;
        }

        public async Task ModificarReser(ReservacionesClientes Param)
        {
            var obtenerData = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones") //comparamos si es la misma key
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Object.Id_Cliente == Param.Id_Cliente).FirstOrDefault();

            await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .Child(obtenerData.Key)
                .PutAsync(new ReservacionesClientes()
                {
                    Id_Cliente = Param.Id_Cliente,
                    Estado = Param.Estado,
                    Precio = Param.Precio,
                    Nombre_Usuario = Param.Nombre_Usuario,
                    Nombre_Estilisita = Param.Nombre_Estilisita,
                    Tipo_Reservacion = Param.Tipo_Reservacion,
                    Hora_Reservacion = Param.Hora_Reservacion,
                    Fecha_Reservacion = Param.Fecha_Reservacion,
                    Calificacion = Param.Calificacion
                });
        }

        public async Task<List<ReservacionesClientes>> GetDatReserva(ReservacionesClientes Param)
        {
            Reservaciones = new List<ReservacionesClientes>();
            var Datos = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Object.Id_Cliente == Param.Id_Cliente);

            foreach (var pd in Datos)
            {
                var Re = new ReservacionesClientes();
                Re.Id_Reservaciones = pd.Key;
                Re.Id_Cliente = pd.Object.Id_Cliente;
                Re.Nombre_Usuario = pd.Object.Nombre_Usuario;
                Re.Nombre_Estilisita = pd.Object.Nombre_Estilisita;
                Re.Hora_Reservacion = pd.Object.Hora_Reservacion;
                Re.Fecha_Reservacion = pd.Object.Fecha_Reservacion;
                Re.Calificacion = pd.Object.Calificacion;
                Re.Estado = pd.Object.Estado;
                Re.Precio = pd.Object.Precio;
                Re.Tipo_Reservacion = pd.Object.Tipo_Reservacion;

                Reservaciones.Add(Re);
            }
            return Reservaciones;
        }

        public async Task UserPostReservacion(ReservacionesClientes datos)
        {
            var Data = (await FirebaseConnection.conexionFirebase
                    .Child("Reservaciones")
                    .OnceAsync<ReservacionesClientes>()).Where(a => a.Key == datos.Id_Reservaciones)
                    .Where(a => a.Object.Id_Cliente == datos.Id_Cliente).FirstOrDefault();
            Data.Object.Nombre_Usuario = datos.Nombre_Usuario;

            await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .Child(Data.Key)
                .PutAsync(Data.Object);
        }

        public async Task<List<ReservacionesClientes>> GetDataToday(ReservacionesClientes Param)
        {
            var data = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Object.Id_Cliente == Param.Id_Cliente)
                .Where(b => b.Object.Fecha_Reservacion == Param.Fecha_Reservacion);
            foreach (var rdr in data)
            {
                var ope = new ReservacionesClientes();
                ope.Id_Reservaciones = rdr.Key;
                ope.Id_Cliente = rdr.Object.Id_Cliente;
                ope.Nombre_Usuario = rdr.Object.Nombre_Usuario;
                ope.Nombre_Estilisita = rdr.Object.Nombre_Estilisita;
                ope.Hora_Reservacion = rdr.Object.Hora_Reservacion;
                ope.Fecha_Reservacion = rdr.Object.Fecha_Reservacion;
                ope.Calificacion = rdr.Object.Calificacion;
                ope.Estado = rdr.Object.Estado;
                ope.Precio = rdr.Object.Precio;
                ope.Tipo_Reservacion = rdr.Object.Tipo_Reservacion;

                Reservaciones.Add(ope);
            }
            return Reservaciones;
        }

        public async Task EditResena(Resenas IdR)
        {
            var data = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Key == IdR.Id_Reservacion).FirstOrDefault();
            data.Object.Calificacion = int.Parse(IdR.Calificacion);
            await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .Child(data.Key)
                .PutAsync(data.Object);
        }

        public async Task EstablecerEstado(ReservacionesClientes IdR)
        {
            var data = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Key == IdR.Id_Reservaciones).FirstOrDefault();
            data.Object.Estado = IdR.Estado;
            await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .Child(data.Key)
                .PutAsync(data.Object);
        }

        public async Task DeleteReservacion(ReservacionesClientes Param)
        {
            var data = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .OnceAsync<ReservacionesClientes>()).Where((a) => a.Key == Param.Id_Reservaciones).FirstOrDefault();
            await FirebaseConnection.conexionFirebase.Child("Reservaciones").Child(data.Key).DeleteAsync();
        }

        public async Task deleteSite(string Id)
        {
            int id = Convert.ToInt32(Id);
            var toDelete = (await FirebaseConnection.conexionFirebase.Child("Reservaciones").OnceAsync<ReservacionesClientes>()).FirstOrDefault
                (a => a.Object.Id_Reservaciones == Id);
            await FirebaseConnection.conexionFirebase.Child("Reservaciones").Child(toDelete.Key).DeleteAsync();
        }


        public async Task<List<ReservacionesClientes>> ObtenerReserEstilista(ReservacionesClientes Param)
        {
            var Datos = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Object.Nombre_Estilisita == Param.Nombre_Estilisita)
                .Where(b => b.Object.Fecha_Reservacion == Param.Fecha_Reservacion)
                .Where(c => c.Object.Estado == Param.Estado);
            foreach (var pda in Datos)
            {
                var Obt = new ReservacionesClientes();
                Obt.Id_Reservaciones = pda.Key;
                Obt.Id_Cliente = pda.Object.Id_Cliente;
                Obt.Nombre_Usuario = pda.Object.Nombre_Usuario;
                Obt.Nombre_Estilisita = pda.Object.Nombre_Estilisita;
                Obt.Hora_Reservacion = pda.Object.Hora_Reservacion;
                Obt.Fecha_Reservacion = pda.Object.Fecha_Reservacion;
                Obt.Calificacion = pda.Object.Calificacion;
                Obt.Estado = pda.Object.Estado;
                Obt.Precio = pda.Object.Precio;
                Obt.Tipo_Reservacion = pda.Object.Tipo_Reservacion;

                Reservaciones.Add(Obt);
            }

            return Reservaciones;
        }


        public async Task<List<ReservacionesClientes>> GetDataGeneEstilista(ReservacionesClientes Param)
        {
            var Datos = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Object.Nombre_Estilisita == Param.Nombre_Estilisita);
            foreach (var pda in Datos)
            {
                var Obt = new ReservacionesClientes();
                Obt.Id_Reservaciones = pda.Key;
                Obt.Id_Cliente = pda.Object.Id_Cliente;
                Obt.Nombre_Usuario = pda.Object.Nombre_Usuario;
                Obt.Nombre_Estilisita = pda.Object.Nombre_Estilisita;
                Obt.Hora_Reservacion = pda.Object.Hora_Reservacion;
                Obt.Fecha_Reservacion = pda.Object.Fecha_Reservacion;
                Obt.Calificacion = pda.Object.Calificacion;
                Obt.Estado = pda.Object.Estado;
                Obt.Precio = pda.Object.Precio;
                Obt.Tipo_Reservacion = pda.Object.Tipo_Reservacion;

                Reservaciones.Add(Obt);
            }

            return Reservaciones;
        }
    }
}
