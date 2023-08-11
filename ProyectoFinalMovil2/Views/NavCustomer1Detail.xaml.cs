using Firebase.Auth;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using ProyectoFinalMovil2.Controllers;
using ProyectoFinalMovil2.Models;
using ProyectoFinalMovil2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavCustomer1Detail : ContentPage
    {
        public NavCustomer1Detail()
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
                pass = fila.Contrasena;
                rutafoto = fila.ImagenPerfil;
                IdUsuariosClientes = fila.Id_User_Cliente;
                nombre = fila.Nombres;
            }

        }

        private  void btnEditPerfil_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Profile());

        }

        private void btnCerrarSesion_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("MyFirebaseRefreshToken");
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}