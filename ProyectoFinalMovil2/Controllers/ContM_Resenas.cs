using Firebase.Database.Query;
using ProyectoFinalMovil2.Models;
using ProyectoFinalMovil2.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalMovil2.Controllers
{
    public class ContM_Resenas
    {
        public async Task InsertResenas(Resenas Param)
        {
            await FirebaseConnection.conexionFirebase
                .Child("Resenas")
                .PostAsync(new Resenas()
                {
                    Calificacion = Param.Calificacion,
                    Id_Usuario = Param.Id_Usuario,
                    Resena = Param.Resena,
                    Id_Reservacion = Param.Id_Reservacion,
                });
        }

        public async Task EditResena(Resenas Param)
        {
            var data = (await FirebaseConnection.conexionFirebase
            .Child("Resenas")
                .OnceAsync<Resenas>()).Where(a => a.Key == Param.Id_Resena).FirstOrDefault();
            data.Object.Resena = Param.Resena;
            data.Object.Calificacion = Param.Calificacion;
            await FirebaseConnection.conexionFirebase
                .Child("Resenas")
                .Child(data.Key)
            .PutAsync(data.Object);
        }

        public async Task<List<Resenas>> GetDatos(Resenas Param)
        {
            var Resenias = new List<Resenas>();
            var Datos = (await FirebaseConnection.conexionFirebase
            .Child("Resenias")
            .OrderByKey()
                .OnceAsync<Resenas>()).Where(a => a.Object.Id_Reservacion == Param.Id_Reservacion);
            foreach (var d in Datos)
            {
                var Pa = new Resenas();
                Pa.Resena = d.Object.Resena;
                Pa.Calificacion = d.Object.Calificacion;
                Pa.Id_Resena = d.Key;
                Resenias.Add(Pa);
            }
            return Resenias;
        }
    }
}
