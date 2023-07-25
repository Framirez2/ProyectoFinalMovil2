using Firebase.Auth;
using Newtonsoft.Json;
using ProyectoFinalMovil2.Controllers;
using ProyectoFinalMovil2.Models;
using ProyectoFinalMovil2.Services;
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
        Usuarios DatosUser = new Usuarios();
        string Id_User;
        string Nombre_Estilista;
        string Fecha;
        string Tipo_User;
        public Historial()
        {
            InitializeComponent();
            GetIdUser();
            validarTipoUser();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await validarTipoUser();
        }

        private async Task GetIdUser()
        {
            try
            {
                var AutentProv = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));
                var Save_Id = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                Id_User = Save_Id.User.LocalId;

                await validarTipoUser();
            }
            catch (Exception)
            {
                await DisplayAlert("Aviso", "La sesion ha expirado", "OK");
            }
        }

        private async void lstGeneral_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReservacionesClientes itemSelect = new ReservacionesClientes();
            ContM_Reservaciones consulta = new ContM_Reservaciones();

            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                itemSelect = e.CurrentSelection[0] as ReservacionesClientes;
            }

            if (Tipo_User == "Empleado")
            {
                string selection = await DisplayActionSheet("Seleccione una opción", null, null, "Eliminar", "Dar de alta");

                if (selection == "Dar de alta")
                {
                    itemSelect.Estado = "Finalizada";
                    await consulta.EstablecerEstado(itemSelect);
                    await validarTipoUser();
                }
                else if (selection == "Eliminar")
                {
                    ContM_Reservaciones funcion = new ContM_Reservaciones();
                    ReservacionesClientes parametros = new ReservacionesClientes();
                    parametros.Id_Reservaciones = itemSelect.Id_Reservaciones;
                    await funcion.DeleteReservacion(parametros);
                    await DisplayAlert("Aviso", "Reservacion eliminada", "OK");
                    await validarTipoUser();
                }
            }
            else if (Tipo_User == "Cliente")
            {
                if (itemSelect.Estado == "Finalizada")
                {
                    string selection = await DisplayActionSheet("Seleccione una opción", null, null, "Calificar");

                    if (selection == "Calificar")
                    {
                        //await PopupNavigation.Instance.PushAsync(new Calificar(itemSelect));
                        await validarTipoUser();
                    }
                }
                else if (itemSelect.Estado == "Pendiente")
                {
                    string selection = await DisplayActionSheet("Seleccione una opción", null, null, "Eliminar");

                    if (selection == "Eliminar")
                    {

                        ContM_Reservaciones funcion = new ContM_Reservaciones();
                        ReservacionesClientes parametros = new ReservacionesClientes();
                        parametros.Id_Reservaciones = itemSelect.Id_Reservaciones;
                        await funcion.DeleteReservacion(parametros);
                        await DisplayAlert("Aviso", "La reservacion se ha eliminado", "OK");
                        await validarTipoUser();
                    }
                }
            }
        }

        private async Task validarTipoUser()
        {
            ContM_Usuarios funcion = new ContM_Usuarios();
            Usuarios parametro = new Usuarios();
            parametro.Id_User = Id_User;
            var Datos = await funcion.GetAdmin(parametro);

            foreach (var a in Datos)
            {
                Tipo_User = a.Tipo_Usuario;
                Nombre_Estilista = a.Nombres;
            }

            Fecha = DateTime.Now.ToString("d/M/yyyy");

            if (Tipo_User == "Cliente")
            {
                ContM_Reservaciones Consulta = new ContM_Reservaciones();
                ReservacionesClientes Param3 = new ReservacionesClientes();
                Param3.Id_Cliente = Id_User;
                lstGeneral.ItemsSource = await Consulta.GetDatReserva(Param3);

                ContM_Reservaciones Consulta2 = new ContM_Reservaciones();
                ReservacionesClientes Param2 = new ReservacionesClientes();
                Param2.Id_Cliente = Id_User;
                Param2.Fecha_Reservacion = Fecha;
            }
            else if (Tipo_User == "admin")
            {
                string FechaA = DateTime.Now.ToString("d/M/yyyy");

                ContM_Reservaciones Consulta = new ContM_Reservaciones();
                ContM_Reservaciones Consulta2 = new ContM_Reservaciones();
                ReservacionesClientes Param1 = new ReservacionesClientes();
                Param1.Fecha_Reservacion = FechaA;

                lstGeneral.ItemsSource = await Consulta.ObtenerReservaciones(Param1);
            }
            else if (Tipo_User == "Empleado")
            {
                string Fech = DateTime.Now.ToString("d/M/yyyy");

                ContM_Reservaciones Consulta = new ContM_Reservaciones();
                ContM_Reservaciones Consulta2 = new ContM_Reservaciones();
                ReservacionesClientes Param1 = new ReservacionesClientes();
                Param1.Fecha_Reservacion = Fech;
                Param1.Estado = "Pendiente";
                Param1.Nombre_Estilisita = Nombre_Estilista;

                lstGeneral.ItemsSource = await Consulta.GetDataGeneEstilista(Param1);
            }
        }

        private async void SwipeItemEliminar_Clicked(object sender, EventArgs e)
        {
            // Mostrar una alerta con la opción de confirmación para eliminar el campo
            var resp = await DisplayAlert("Aviso", "Desea eliminar el campo?", "Si", "No");
            if (resp)
            {
                // Obtener el SwipeItem (elemento deslizable) que desencadenó el evento
                SwipeItem item = sender as SwipeItem;
                var Id = item.CommandParameter.ToString();
                if (Id != null)
                {
                    ContM_Reservaciones fun = new ContM_Reservaciones();

                    await fun.deleteSite(Id);

                    
                }
                else await DisplayAlert("Error", "error", "ok");
            }
            else
            {
                await DisplayAlert("Error", "Ha ocurrido un error eliminando el sitio", "Ok");
            }
        }



        /* private async Task validarTipoUser()
         {
             ContM_Usuarios funcion = new ContM_Usuarios();
             Usuarios parametro = new Usuarios();
             parametro.Id_User = Id_User;
             var Datos = await funcion.GetAdmin(parametro);

             foreach (var a in Datos)
             {
                 Tipo_User = a.Tipo_Usuario;
                 Nombre_Estilista = a.Nombres;
             }

             Fecha = DateTime.Now.ToString("d/M/yyyy");

             ContM_Reservaciones consulta = new ContM_Reservaciones();
             ReservacionesClientes param = new ReservacionesClientes();

             switch (Tipo_User)
             {
                 case "Cliente":
                     param.Id_Cliente = Id_User;
                     lstGeneral.ItemsSource = await consulta.GetDatReserva(param);

                     param = new ReservacionesClientes
                     {
                         Id_Cliente = Id_User,
                         Fecha_Reservacion = Fecha
                     };
                     lstGeneral.ItemsSource = await consulta.GetDatReserva(param);
                     break;
                 case "admin":
                     param.Fecha_Reservacion = Fecha;
                     lstGeneral.ItemsSource = await consulta.ObtenerReservaciones(param);
                     break;
                 case "Empleado":
                     param.Fecha_Reservacion = Fecha;
                     param.Estado = "Pendiente";
                     param.Nombre_Estilisita = Nombre_Estilista;
                     lstGeneral.ItemsSource = await consulta.GetDataGeneEstilista(param);
                     break;
                 default:
                     // Manejo de caso no esperado (opcional)
                     break;
             }
         }*/


    }


}