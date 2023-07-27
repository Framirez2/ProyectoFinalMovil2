using Firebase.Auth;
using Newtonsoft.Json;
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
    public partial class Dashboard : ContentPage
    {
        string Id_User;
        string Nombre_Estilista;
        string Fecha;
        string Tipo_User;
        public Dashboard()
        {
            InitializeComponent();
            _ = GetIdUser();
            _ = ValidarTipoUser();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await ValidarTipoUser();
        }

        private async Task GetIdUser()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));
                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                Id_User = guardarId.User.LocalId;
                await ValidarTipoUser();
            }
            catch (Exception)
            {
                await DisplayAlert("Alerta", "Sesion expirada", "OK");
            }
        }

        private async Task ValidarTipoUser()
        {
            ContM_Usuarios Funcion = new ContM_Usuarios();
            Usuarios parametro = new Usuarios();
            parametro.Id_User = Id_User;
            var datos = await Funcion.GetAdmin(parametro);

            Tipo_User = datos.FirstOrDefault()?.Tipo_Usuario;
            Nombre_Estilista = datos.FirstOrDefault()?.Nombres;

            Fecha = DateTime.Now.ToString("d/M/yyyy");
            encabezado1.Text = "Reservaciones pendientes para Hoy!!";

            ContM_Reservaciones consulta2 = new ContM_Reservaciones();
            ReservacionesClientes parametro1 = new ReservacionesClientes();
            parametro1.Fecha_Reservacion = Fecha;

            if (Tipo_User == "Cliente")
            {
                encabezado1.Text = "Reservaciones pendientes para Hoy";
                parametro1.Id_Cliente = Id_User;
                lstReserUser.ItemsSource = await consulta2.GetDataToday(parametro1);
            }
            else if (Tipo_User == "admin")
            {
                lstReserUser.ItemsSource = await consulta2.GetDateReserv(Fecha, "Pendiente");
            }
            else if (Tipo_User == "Empleado")
            {
                parametro1.Estado = "Pendiente";
                parametro1.Nombre_Estilisita = Nombre_Estilista;
                lstReserUser.ItemsSource = await consulta2.ObtenerReservacionesEstilista(parametro1);
            }
        }

    }
}