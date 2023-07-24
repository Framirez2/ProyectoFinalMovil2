using Firebase.Auth;
using Firebase.Database.Query;
using Firebase.Storage;
using ProyectoFinalMovil2.Models;
using ProyectoFinalMovil2.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ProyectoFinalMovil2.Controllers
{
    public class ContM_Usuarios
    {
        List<Usuarios> Usuarios = new List<Usuarios>();

        string Rutafoto;
        string IdUsuario_Cliente;

        public async Task<List<Usuarios>> VerUsuarios()
        {
            var data = await FirebaseConnection.conexionFirebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<Usuarios>();

            return data.Select(pa => new Usuarios
            {
                Id_User = pa.Key,
                Nombres = pa.Object.Nombres,
                Contrasena = pa.Object.Contrasena,
                ImagenPerfil = pa.Object.ImagenPerfil,
                Correo = pa.Object.Correo,
                Id_User_Cliente = pa.Object.Id_User_Cliente
            }).ToList();
        }


        /* public async Task<string> InsertarUsuario(Usuarios parametros)
         {
             var data = await FirebaseConnection.conexionFirebase
                   .Child("Usuarios")
                   .PostAsync(new Usuarios()
                   {
                       Id_User = parametros.Id_User,
                       Id_User_Cliente = parametros.Id_User_Cliente,
                       Nombres = parametros.Nombres,
                       Apellidos = parametros.Apellidos,
                       Contrasena = parametros.Contrasena,
                       ImagenPerfil = parametros.ImagenPerfil,
                       Correo = parametros.Correo,
                       Tipo_Usuario = parametros.Tipo_Usuario

                   });

             IdUsuario_Cliente = data.Key;
             return IdUsuario_Cliente;
         }*/

        public async Task<string> InsertarUsuario(Usuarios parametros)
        {
            var data = await FirebaseConnection.conexionFirebase
                .Child("UsuariosClientes")
                .PostAsync(parametros);

            return data.Object?.Id_User; // Suponiendo que la propiedad Id_User contiene el ID del usuario
        }


        /*  public async Task<string> SubirImagenStorage(Stream ImagenStream, string Idusuarios)
          {
              var dataAlmacenamiento = await new FirebaseStorage("mov2nailbarfirebase.appspot.com")
                  .Child("Usuarios")
                  .Child(Idusuarios + ".png")
                  .PutAsync(ImagenStream);
              Rutafoto = dataAlmacenamiento;
              return Rutafoto;
          }*/

        public async Task<string> SubirImagenStorage(Stream ImagenStream, string Idusuarios)
        {
            var storage = new FirebaseStorage("mov2nailbarfirebase.appspot.com");

            var imagenPath = $"UsuariosClientes/{Idusuarios}.png";
            var dataAlmacenamiento = await storage
                .Child(imagenPath)
                .PutAsync(ImagenStream);

            return dataAlmacenamiento;
        }


        /*public async Task EditarFotoPerfil(Usuarios parametros)
        {
            var obtenerData = (await FirebaseConnection.conexionFirebase
                .Child("Usuarios") //comparamos si es la misma key
                .OnceAsync<Usuarios>()).Where(a => a.Key == parametros.Id_User_Cliente).FirstOrDefault();

            await FirebaseConnection.conexionFirebase
                .Child("Usuarios")
                .Child(obtenerData.Key)
                .PutAsync(new Usuarios()
                {
                    Id_User_Cliente = parametros.Id_User_Cliente,
                    Id_User = parametros.Id_User,
                    Nombres = parametros.Nombres,
                    Apellidos = parametros.Apellidos,
                    Contrasena = parametros.Contrasena,
                    ImagenPerfil = parametros.ImagenPerfil,
                    Correo = parametros.Correo,
                    Tipo_Usuario = parametros.Tipo_Usuario
                });
        }*/

        public async Task EditarFotoPerfil(Usuarios parametros)
        {
            var obtenerData = (await FirebaseConnection.conexionFirebase
                .Child("Usuarios")
                .OnceAsync<Usuarios>()).FirstOrDefault(a => a.Key == parametros.Id_User_Cliente);

            if (obtenerData != null)
            {
                var usuarioActualizado = new Usuarios
                {
                    Id_User_Cliente = parametros.Id_User_Cliente,
                    Id_User = parametros.Id_User,
                    Nombres = parametros.Nombres,
                    Contrasena = parametros.Contrasena,
                    ImagenPerfil = parametros.ImagenPerfil,
                    Correo = parametros.Correo,
                    Tipo_Usuario = parametros.Tipo_Usuario
                };

                await FirebaseConnection.conexionFirebase
                    .Child("UsuariosClientes")
                    .Child(obtenerData.Key)
                    .PutAsync(usuarioActualizado);
            }
        }


        /* public async Task<bool> EliminarUsuario(string Cliente_Id)
         {
             try
             {
                 var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));
                 await authProvider.DeleteUserAsync(Preferences.Get("MyToken", "default_value"));
                 await FirebaseConnection.conexionFirebase.Child("Usuarios/" + Cliente_Id).DeleteAsync();
                 return true;
             }
             catch (Exception)
             {
                 return false;
             }
         }*/

        public async Task<bool> EliminarUsuario(string Cliente_Id)
        {
            try
            {
                // Eliminar el usuario de Firebase Authentication
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));
                var token = Preferences.Get("MyToken", "default_value");
                await authProvider.DeleteUserAsync(token);

                // Eliminar el usuario de la base de datos de Firebase
                await FirebaseConnection.conexionFirebase
                    .Child("UsuariosClientes")
                    .Child(Cliente_Id)
                    .DeleteAsync();

                return true; // El usuario se eliminó exitosamente
            }
            catch (Exception)
            {
                return false; // Ocurrió un error al eliminar el usuario
            }
        }


        /*public async Task<bool> CambiarContrasena(string newPassword)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));
                await authProvider.ChangeUserPassword(Preferences.Get("MyToken", "default_value"), newPassword);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }*/

        public async Task<bool> CambiarContrasena(string newPassword)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));
                var token = Preferences.Get("MyToken", "default_value");

                // Cambiar la contraseña del usuario en Firebase Authentication
                await authProvider.ChangeUserPassword(token, newPassword);

                return true; // La contraseña se cambió exitosamente
            }
            catch (Exception)
            {
                return false; // Ocurrió un error al cambiar la contraseña
            }
        }


        /* public async Task EliminarFotoPerfil(string Nombre)
         {
             await new FirebaseStorage("mov2nailbarfirebase.appspot.com")
                 .Child("Usuarios")
                 .Child(Nombre)
                 .DeleteAsync();
         }*/

        public async Task EliminarFotoPerfil(string Nombre)
        {
            var storage = new FirebaseStorage("mov2nailbarfirebase.appspot.com");

            var imagenPath = $"UsuariosClientes/{Nombre}";
            await storage
                .Child(imagenPath)
                .DeleteAsync();
        }

        public async Task GuardarTrabajador(Trabajadores DatosTra)
        {
            // La variable 'data' almacenará la respuesta de la operación.
            var data = await FirebaseConnection.conexionFirebase
                  .Child("Trabajadores")
                  .PostAsync(new Trabajadores()
                  {
                      // Establecemos las propiedades del objeto Trabajadores que se va a guardar.
                      // Utilizamos los valores pasados en el parámetro 'DatosTra' para establecer Nombres y FotoPerfil.
                      Nombres = DatosTra.Nombres,
                      Foto_Perfil = DatosTra.Foto_Perfil
                  });
        }


        public async Task EditTrabajador(Trabajadores Param)
        {
            var data = (await FirebaseConnection.conexionFirebase
                 .Child("Trabajadores")
                 .OnceAsync<Trabajadores>()).Where(a => a.Object.Nombres == Param.Nombres).FirstOrDefault();
            data.Object.Nombres = Param.Nombres;
            await FirebaseConnection.conexionFirebase
                .Child("Trabajadores")
                .Child(data.Key)
                .PutAsync(data.Object);
        }


        public async Task<List<Usuarios>> GetDataUser(Usuarios Param)
        {
            var data = (await FirebaseConnection.conexionFirebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<Usuarios>()).Where(a => a.Object.Id_User == Param.Id_User);
            foreach (var pa in data)
            {
                Param.Id_User = pa.Object.Id_User;
                Param.Nombres = pa.Object.Nombres;
                Param.Contrasena = pa.Object.Contrasena;
                Param.ImagenPerfil = pa.Object.ImagenPerfil;
                Param.Correo = pa.Object.Correo;

                Usuarios.Add(Param);
            }
            return Usuarios;
        }

        public async Task<List<Usuarios>> GetDataMail(Usuarios Param)
        {
            var data = (await FirebaseConnection.conexionFirebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<Usuarios>()).Where(a => a.Object.Id_User == Param.Id_User);
            foreach (var pa in data)
            {
                Param.Tipo_Usuario = pa.Object.Tipo_Usuario;

                Usuarios.Add(Param);
            }
            return Usuarios;
        }

        public async Task<List<Usuarios>> GetDataMail1(Usuarios Param)
        {
            var data = (await FirebaseConnection.conexionFirebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<Usuarios>()).Where(a => a.Object.Correo == Param.Correo);
            foreach (var pa in data)
            {
                Param.Tipo_Usuario = pa.Object.Tipo_Usuario;

                Usuarios.Add(Param);
            }
            return Usuarios;
        }

        public async Task<List<Usuarios>> GetDataType(Usuarios Param)
        {
            var data = (await FirebaseConnection.conexionFirebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<Usuarios>()).Where(a => a.Object.Id_User_Cliente == Param.Id_User_Cliente);
            foreach (var rdr in data)
            {
                Param.Tipo_Usuario = rdr.Object.Tipo_Usuario;

                Usuarios.Add(Param);
            }
            return Usuarios;
        }

        public async Task<List<Usuarios>> GetDataProfile(Usuarios Param)
        {
            var data = (await FirebaseConnection.conexionFirebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<Usuarios>()).Where(a => a.Object.Id_User == Param.Id_User);
            foreach (var pa in data)
            {
                Param.Id_User = pa.Object.Id_User;
                Param.Id_User_Cliente = pa.Object.Id_User_Cliente;
                Param.Nombres = pa.Object.Nombres;
                Param.Contrasena = pa.Object.Contrasena;
                Param.ImagenPerfil = pa.Object.ImagenPerfil;
                Param.Correo = pa.Object.Correo;

                Usuarios.Add(Param);
            }
            return Usuarios;
        }

        public async Task<List<ReservacionesClientes>> ContarReservaciones(ReservacionesClientes Param, string fecha)
        {
            var Contar = new List<ReservacionesClientes>();
            var data = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Object.Id_Cliente == Param.Id_Cliente)
                .Where(b => b.Object.Fecha_Reservacion == fecha);

            foreach (var pa in data)
            {
                Param.Id_Cliente = pa.Object.Id_Cliente;

                Contar.Add(Param);
                Contar.Count();
            }
            return Contar;
        }

        public async Task<List<ReservacionesClientes>> NumeroReserEstilista(ReservacionesClientes Param, string fecha)
        {
            var Contar = new List<ReservacionesClientes>();
            var data = (await FirebaseConnection.conexionFirebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<ReservacionesClientes>()).Where(a => a.Object.Nombre_Estilisita == Param.Nombre_Estilisita)
                .Where(b => b.Object.Fecha_Reservacion == fecha);

            foreach (var pa in data)
            {
                Param.Id_Cliente = pa.Object.Id_Cliente;

                Contar.Add(Param);
                Contar.Count();
            }
            return Contar;
        }


        public async Task<List<Usuarios>> GetAdmin(Usuarios Param)
        {
            var data = (await FirebaseConnection.conexionFirebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<Usuarios>()).Where(a => a.Object.Id_User == Param.Id_User);

            foreach (var pa in data)
            {
                var Nombre = new Usuarios();

                Nombre.Tipo_Usuario = pa.Object.Tipo_Usuario;
                Nombre.Id_User_Cliente = pa.Key;
                Nombre.Nombres = pa.Object.Nombres;

                Usuarios.Add(Nombre);
            }
            return Usuarios;
        }
    }
}
