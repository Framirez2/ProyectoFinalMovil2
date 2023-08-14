using Acr.UserDialogs;
using Firebase.Auth;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Plugin.Media;
using ProyectoFinalMovil2.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProyectoFinalMovil2.Controllers;
using ProyectoFinalMovil2.Services;
using Rg.Plugins.Popup.Extensions;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        public Profile()
        {
            InitializeComponent();
            ObtenerIdusuario();
        }

        MediaFile file;
        string rutafoto;
        string Iduserlogin;
        string pass;
        string IdUsuariosClientes;
        string estado = "vacio";
        string nombre;
        string tipoUser;
        string nombreEdit;

        /*
        
        string nombreEstilista;
        string status;
        string hora_Reserv;
        int calificacion;
        string fecha_Reserv;
        string tipo_Reserv;*/

        private async void btnActualizar_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Espere por favor...",MaskType.Gradient);
            // Verificar si los campos están vacíos
            if (string.IsNullOrWhiteSpace(nombres.Text) || string.IsNullOrWhiteSpace(email.Text))
            {
                await DisplayAlert("Error", "Por favor, complete todos los campos obligatorios.", "OK");
                return; // Salir del evento si los campos están vacíos
            }

            if (estado == "lleno" || nombre != nombres.Text)
            {
                ContM_Usuarios funcion2 = new ContM_Usuarios();
                Usuarios parametros = new Usuarios();
                parametros.Id_User_Cliente = IdUsuariosClientes;
                var dt = await funcion2.GetDataType(parametros);
                foreach (var fila in dt)
                {
                    tipoUser = fila.Tipo_Usuario;
                }

                await EditarFoto();
            }
            else
            {
                await DisplayAlert("Aviso", "No se encontraron cambios", "Ok");
            }

            UserDialogs.Instance.HideLoading();
        }


        private async void btnActualizarpass_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Espere por favor...",MaskType.Gradient);
            await EditPass();
            UserDialogs.Instance.HideLoading();
        }

        private async Task EditarFoto()
        {
            if (tipoUser == "Empleado")
            {
                ContM_Usuarios funcion = new ContM_Usuarios();
                Usuarios parametros = new Usuarios();

                parametros.Id_User_Cliente = IdUsuariosClientes;
                parametros.Id_User = Iduserlogin;
                parametros.Contrasena = pass;
                parametros.Nombres = nombres.Text;
                parametros.ImagenPerfil = rutafoto;
                parametros.Correo = email.Text;
                parametros.Tipo_Usuario = tipoUser;

                await funcion.EditarFotoPerfil(parametros);
                await ObtenerDatoReservacion();
            }
            else
            {
                ContM_Usuarios funcion = new ContM_Usuarios();
                Usuarios parametros = new Usuarios();

                parametros.Id_User_Cliente = IdUsuariosClientes;
                parametros.Id_User = Iduserlogin;
                parametros.Contrasena = pass;
                parametros.Nombres = nombres.Text;
                parametros.ImagenPerfil = rutafoto;
                parametros.Correo = email.Text;
                parametros.Tipo_Usuario = tipoUser;

                await funcion.EditarFotoPerfil(parametros);
                await ObtenerDatoReservacion();
            }
            await DisplayAlert("Aviso", "Los datos se han actualizado satisfactoriamente.", "OK");
        }

        private async void btnagregarimagen_Clicked(object sender, EventArgs e)
        {

            // animation.IsVisible = false;
            // imagenCelular.IsVisible = true;

            await CrossMedia.Current.Initialize();
            try
            {
                //para poder acceder a la galeria y agregar la img que queramos  NOTA: NIVEL LOCAL aun no se sube la imagen
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null)
                {

                    return;
                }
                else
                {
                    btnsele.Source = ImageSource.FromStream(() =>
                    {
                        //GetStream extrae toda la ruta de la imagen
                        var rutaImagen = file.GetStream();

                        return rutaImagen;
                    });
                    await SubirImagenesStore();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task SubirImagenesStore()
        {
            ContM_Usuarios funcion = new ContM_Usuarios();
            rutafoto = await funcion.SubirImagenStorage(file.GetStream(), Iduserlogin);

            estado = "lleno";

        }

        private async Task EditUser()
        {

            if (tipoUser == "Empleado")
            {


                ContM_Usuarios funcion = new ContM_Usuarios();
                Usuarios parametros = new Usuarios();

                parametros.Id_User_Cliente = IdUsuariosClientes;
                parametros.Id_User = Iduserlogin;
                parametros.Nombres = nombres.Text;
                parametros.ImagenPerfil = rutafoto;
                parametros.Contrasena = pass;
                parametros.Correo = email.Text;
                parametros.Tipo_Usuario = tipoUser;

                await funcion.EditarFotoPerfil(parametros);
                await ObtenerDatoReservacion();

            }
            else
            {
                ContM_Usuarios funcion = new ContM_Usuarios();
                Usuarios parametros = new Usuarios();

                parametros.Id_User_Cliente = IdUsuariosClientes;
                parametros.Id_User = Iduserlogin;
                parametros.Nombres = nombres.Text;
                parametros.ImagenPerfil = rutafoto;
                parametros.Contrasena = pass;
                parametros.Correo = email.Text;
                parametros.Tipo_Usuario = tipoUser;

                await funcion.EditarFotoPerfil(parametros);
                await ObtenerDatoReservacion();
            }




            await DisplayAlert("Advertencia", "Datos Acualizados", "OK");

        }
        private async Task EditPass()
        {
            if (string.IsNullOrEmpty(contra.Text) || string.IsNullOrEmpty(contranue.Text))
            {
                await DisplayAlert(null, "Error: campos vacios", "Aceptar");
                return;
            }

            if (contra.Text == contranue.Text)
            {
                if (contra.Text.Length >= 6)
                {
                    ContM_Usuarios funcion = new ContM_Usuarios();
                    await funcion.CambiarContrasena(contranue.Text);
                    await ObtenerDatoReservacion();
                    await DisplayAlert("Advertencia", "Contraseña Acualizada", "OK");
                }
                else
                {
                    await DisplayAlert("Advertencia", "La contraseña debe ser mayor de 6 caracteres", "OK");
                }
            }
            else
            {
                await DisplayAlert("Advertencia", "Contraseñas no coinciden", "OK");
            }

        }

        private async Task ObtenerDatoReservacion()
        {
            ContM_Reservaciones obtener2 = new ContM_Reservaciones();
            ContM_Reservaciones consulta = new ContM_Reservaciones();
            ReservacionesClientes parametros = new ReservacionesClientes();
            parametros.Id_Cliente = Iduserlogin;
            parametros.Nombre_Usuario = nombres.Text;
            int contador = 0;
            var dt = await obtener2.ObtenerReservaciones(parametros);
            foreach (var fila in dt)
            {

                ReservacionesClientes data = new ReservacionesClientes();
                data.Id_Reservaciones = fila.Id_Reservaciones;
                data.Id_Cliente = fila.Id_Cliente;
                data.Nombre_Usuario = nombres.Text;
                await consulta.UserPostReservacion(data);

            }

        }

        /*
        private async Task obtener(MoReservaciones parame)
        {
            VmReservaciones funcionR = new VmReservaciones();
            MoReservaciones parametros2 = new MoReservaciones();

            parametros2.id_Cliente = Iduserlogin;
            parametros2.status = status;
            parametros2.nombreEstilista = nombreEstilista;
            parametros2.hora_Reserv = hora_Reserv;
            parametros2.calificacion = calificacion;
            parametros2.fecha_Reserv = fecha_Reserv;
            parametros2.nombre_usuario = txtNombres.Text;
            parametros2.tipo_Reserv = tipo_Reserv;


            await funcionR.Modificar(parametros2);
        }
        */
        private async Task ObtenerIdusuario()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));
                //validar si el usuario se ha validado o no dentro de la aplicacion
                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                //var RefrescarContenido = await authProvider.RefreshAuthAsync(guardarId);
                //Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefrescarContenido));
                //el ID

                Iduserlogin = guardarId.User.LocalId;

                await ObtenerDatoUsuario();
            }
            catch (Exception)
            {

                await DisplayAlert("Alerta", "Sesion expirada", "OK");
            }


        }

        private async Task ObtenerDatoUsuario()
        {
            ContM_Usuarios funcion = new ContM_Usuarios();
            Usuarios parametros = new Usuarios();
            parametros.Id_User = Iduserlogin;
            var dt = await funcion.GetDataProfile(parametros);
            foreach (var fila in dt)
            {
                nombres.Text = fila.Nombres;
                btnsele.Source = fila.ImagenPerfil;
                email.Text = fila.Correo;
                pass = fila.Contrasena;
                rutafoto = fila.ImagenPerfil;
                IdUsuariosClientes = fila.Id_User_Cliente;
                nombre = fila.Nombres;
            }

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        private void btncerrar_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Cerrando sesion", MaskType.Gradient);
            Preferences.Remove("MyFirebaseRefreshToken");
            Application.Current.MainPage = new NavigationPage(new Login());
            UserDialogs.Instance.HideLoading();
        }

        private async void btnFoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null)
                {
                    return;
                }
                else
                {
                    btnsele.Source = ImageSource.FromStream(() =>
                    {
                        var rutaImagen = file.GetStream();
                        return rutaImagen;
                    });
                    await SubirImagenesStore();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void btnDeleteAcount_Clicked(object sender, EventArgs e)
        {
            bool borrar = await DisplayAlert("Aviso", "¿Desea eliminar su cuenta permanentemente?", "Aceptar", "Cancelar");
            if (borrar)
            {
                //UserDialogs.Instance.ShowLoading("Procesando Solicitud...");
                ContM_Usuarios funcion = new ContM_Usuarios();
                bool response = await funcion.EliminarUsuario(IdUsuariosClientes);

                if (response)
                {
                    await DisplayAlert("Satisfactorio", "Su cuenta se a eliminado satisfactoriamente.", "OK");
                    Preferences.Remove("MyFirebaseRefreshToken");
                    Preferences.Remove("MyToken");
                    Application.Current.MainPage = new NavigationPage(new Login());
                }
                else
                {
                    await DisplayAlert("Aviso", "A ocurrido un error al borrar su cuenta.", "OK");
                }
                //UserDialogs.Instance.HideLoading();
            }
        }
    }
}