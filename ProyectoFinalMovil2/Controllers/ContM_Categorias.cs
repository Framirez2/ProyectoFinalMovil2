using Firebase.Database.Query;
using ProyectoFinalMovil2.Models;
using ProyectoFinalMovil2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalMovil2.Controllers
{
    public class ContM_Categorias
    {
        List<Trabajadores> Estilista = new List<Trabajadores>();
        List<Usuarios> Usuarios = new List<Usuarios>();

        public async Task InsertCategorias(Categorias Param)
        {
            await FirebaseConnection.conexionFirebase
                .Child("Categorias")
                .PostAsync(new Categorias()
                {
                    Categoria = Param.Categoria,
                    Color = Param.Color,
                    Imagen = Param.Imagen,
                    Prioridad = Param.Prioridad
                });
        }

        public async Task InsertTrabajadores(Trabajadores Param)
        {
            await FirebaseConnection.conexionFirebase
                .Child("Trabajadores")
                .PostAsync(new Trabajadores()
                {
                    Nombres = Param.Nombres,
                    Contra = Param.Contra,
                    Foto_Perfil = Param.Foto_Perfil,
                    Correo = Param.Correo,
                    Tipo_User = Param.Tipo_User,
                });
        }

        public async Task<List<Categorias>> ShowNormalCategories()
        {
            var Categorias = new List<Categorias>();
            var funcionNegocios = new ContM_Usuarios();
            var parametrosNegocios = new ContM_CrearCuenta();
            var data = (await FirebaseConnection.conexionFirebase
            .Child("Categorias")
            .OrderByKey()
                .OnceAsync<Categorias>()).Where(a => a.Object.Prioridad == "0");
            foreach (var dt in data)
            {
                Categorias Param = new Categorias();
                Param.Categoria = dt.Object.Categoria;
                Param.Imagen = dt.Object.Imagen;
                Param.Id_Categoria = dt.Key;
                Param.Color = dt.Object.Color;

                Categorias.Add(Param);
            }
            return Categorias;
        }

        public async Task<List<Trabajadores>> ObtenerEstilistas()
        {
            var data = await FirebaseConnection.conexionFirebase
                .Child("Trabajadores")
                .OrderByKey()
                .OnceAsync<Trabajadores>();
            foreach (var Oe in data)
            {
                Trabajadores Param = new Trabajadores();
                Param.Id_Trabajador = Oe.Key;
                Param.Nombres = Oe.Object.Nombres;
                Param.Foto_Perfil = Oe.Object.Foto_Perfil;
                Estilista.Add(Param);
            }
            return Estilista;
        }
    }
}
