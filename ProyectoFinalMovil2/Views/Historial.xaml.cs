using Firebase.Auth;
using Newtonsoft.Json;
using ProyectoFinalMovil2.Controllers;
using ProyectoFinalMovil2.Models;
using ProyectoFinalMovil2.Services;
using Rating;
using Rg.Plugins.Popup.Services;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Historial : ContentPage
    {
        
        
        public Historial()
        {
            InitializeComponent();
            _ = GetIdUser();
            _ = ValidarTipoUser();
            
        }

        string Id_User;
        string Nombre_Estilista;
        string Fecha;
        string Tipo_User;

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await ValidarTipoUser();
        }

        // Este método se encarga de obtener el ID del usuario actualmente autenticado.
        private async Task GetIdUser()
        {
            try
            {
                // Creamos una instancia de la clase FirebaseAuthProvider y le proporcionamos la clave API de Firebase.
                var AutentProv = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));

                // Recuperamos el ID del usuario almacenado previamente en las preferencias de la aplicación.
                // Este ID es obtenido del token de autenticación de Firebase que se guardó durante el inicio de sesión.
                // Lo deserializamos para obtener el objeto FirebaseAuth, que contiene la información del usuario.
                var Save_Id = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));

                // Extraemos el ID del usuario localmente autenticado y lo almacenamos en la variable Id_User.
                Id_User = Save_Id.User.LocalId;

                await ValidarTipoUser();
            }
            catch (Exception)
            {
                await DisplayAlert("Aviso", "La sesion ha expirado", "OK");
            }
        }

        // Este método se ejecuta cuando se selecciona un elemento en la colección de reservaciones.
        private async void lstGeneral_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verificamos si la selección actual es nula o está vacía.
            // Si es así, no realizamos ninguna acción y salimos del método.
            if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
                return;
            // Obtenemos el objeto ReservacionesClientes seleccionado de la colección.
            ReservacionesClientes itemSelect = e.CurrentSelection[0] as ReservacionesClientes;
            ContM_Reservaciones consulta = new ContM_Reservaciones();

            // Verificamos el tipo de usuario actual (Empleado o Cliente).
            if (Tipo_User == "Empleado")
            {
                string selection = await DisplayActionSheet("Seleccione una opción", null, null, "Eliminar", "Dar de alta");

                if (selection == "Dar de alta")
                {
                    itemSelect.Estado = "Finalizada";
                    await consulta.EstablecerEstado(itemSelect);
                }
                else if (selection == "Eliminar")
                {
                    await EliminarReservacion(itemSelect);
                }
            }
            else if (Tipo_User == "Cliente")
            {
                if (itemSelect.Estado == "Finalizada")
                {
                    string selection = await DisplayActionSheet("Seleccione una opción", null, null, "Calificar");

                    if (selection == "Calificar")
                    {
                        await PopupNavigation.Instance.PushAsync(new Rating(itemSelect));
                    }
                }
                else if (itemSelect.Estado == "Pendiente")
                {
                    string selection = await DisplayActionSheet("Seleccione una opción", null, null, "Eliminar");

                    if (selection == "Eliminar")
                    {
                        await EliminarReservacion(itemSelect);
                    }
                }
            }

            await ValidarTipoUser();
        }

        private async Task EliminarReservacion(ReservacionesClientes reservacion)
        {
            ContM_Reservaciones funcion = new ContM_Reservaciones();
            ReservacionesClientes parametros = new ReservacionesClientes();
            parametros.Id_Reservaciones = reservacion.Id_Reservaciones;
            await funcion.DeleteReservacion(parametros);
            await DisplayAlert("Aviso", "Reservacion eliminada", "OK");
        }


        private void btn_Rating_Clicked(object sender, EventArgs e)
        {
            //int selectedRating = rating.SelectedStarValue;
            
        }


        // Este método se encarga de validar el tipo de usuario y cargar los datos correspondientes en la colección lstGeneral.
        private async Task ValidarTipoUser()
        {
            // Creamos una instancia de ContM_Usuarios para obtener los datos del usuario actual.
             ContM_Usuarios funcion = new ContM_Usuarios();
             Usuarios parametro = new Usuarios();
             parametro.Id_User = Id_User;
             var Datos = await funcion.GetAdmin(parametro);

            // Iteramos a través de los datos obtenidos para obtener el Tipo de Usuario y el Nombre del Estilista.
            foreach (var a in Datos)
             {
                 Tipo_User = a.Tipo_Usuario;
                 Nombre_Estilista = a.Nombres;
             }

             Fecha = DateTime.Now.ToString("d/M/yyyy");

            // Creamos una instancia de ContM_Reservaciones para obtener los datos de las reservaciones según el tipo de usuario.
             ContM_Reservaciones consulta = new ContM_Reservaciones();
             ReservacionesClientes param = new ReservacionesClientes();

            // Según el Tipo de Usuario, cargamos los datos en la colección lstGeneral.
            switch (Tipo_User)
             {
                 case "Cliente":
                    // Para los clientes, obtenemos las reservaciones del cliente actual para la fecha actual.
                    param.Id_Cliente = Id_User;
                     lstGeneral.ItemsSource = await consulta.GetDatReserva(param);

                    // También obtenemos las reservaciones pendientes del cliente actual.
                    param = new ReservacionesClientes
                     {
                         Id_Cliente = Id_User,
                         Fecha_Reservacion = Fecha
                     };
                     lstGeneral.ItemsSource = await consulta.GetDatReserva(param);
                     break;
                 case "admin":
                    // Para los usuarios administradores, obtenemos todas las reservaciones para la fecha actual.
                    param.Fecha_Reservacion = Fecha;
                     lstGeneral.ItemsSource = await consulta.ObtenerReservaciones(param);
                     break;
                 case "Empleado":
                    // Para los empleados, obtenemos las reservaciones pendientes para la fecha actual y el nombre del estilista actual.
                    param.Fecha_Reservacion = Fecha;
                     param.Estado = "Pendiente";
                     param.Nombre_Estilisita = Nombre_Estilista;
                     lstGeneral.ItemsSource = await consulta.GetDataGeneEstilista(param);
                     break;
                 default:
                     // Manejo de caso no esperado (opcional)
                     break;
             }
         }


    }


}