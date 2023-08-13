using ProyectoFinalMovil2.Controllers;
using ProyectoFinalMovil2.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup;
using Acr.UserDialogs;
using Xamarin.Essentials;
using Plugin.LocalNotification;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        string Tipo_User;
        string IdUser_Login;
        public Login()
        {
            InitializeComponent();
            // Cargar el correo electrónico guardado en Preferences, si existe
            // Cargar el correo electrónico y la contraseña guardados en Preferences, si existen
            string savedEmail = Preferences.Get("SavedEmail", string.Empty);
            Correo.Text = savedEmail;

            string savedPassword = Preferences.Get("SavedPassword", string.Empty);
            Contra.Text = savedPassword;
        }

        private async void btnlogin_Clicked(object sender, EventArgs e)
        {
            await ValidarDatos();
            // Guardar el correo electrónico ingresado en Preferences
            Preferences.Set("SavedEmail", Correo.Text);
            Preferences.Set("SavedPassword", Contra.Text);

            if (!string.IsNullOrEmpty(Correo.Text))
             {
                 if (!string.IsNullOrEmpty(Contra.Text))
                 {

                     await ValidarDatos();
                 }
                 else
                 {
                    await DisplayAlert("Aviso", "Debe ingresar su contraseña.", "OK");
                 }
             }
             else
             {
                    await DisplayAlert("Aviso", "Debe ingresar su correo.", "OK");

             }

           
        }
        private async void btnregistrar_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new Registro());

        }

        private async Task ValidarDatos()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Atencion", "Verifique su conexion a internet", "Aceptar");
                return;
            }
            else
            {
                try
                {
                    ContM_Usuarios funcion2 = new ContM_Usuarios();
                    Usuarios parametros = new Usuarios();
                    parametros.Correo = Correo.Text;
                    var dt = await funcion2.GetDataMail1(parametros);

                    foreach (var fila in dt)
                    {
                        Tipo_User = fila.Tipo_Usuario;
                    }

                    if (Tipo_User == "Cliente")
                    {
                        var funcion = new ContM_CrearCuenta();
                        await funcion.ValidarCuenta(Correo.Text, Contra.Text);
                        Application.Current.MainPage = new NavigationPage(new NavCustomer1());

                        var notification = new NotificationRequest
                        {
                            BadgeNumber = 1,
                            Description = "Ha iniciado sesion",
                            Title = "Bienvenida!",
                            ReturningData = "Dummy Data",
                            NotificationId = 1337,

                        };

                        await LocalNotificationCenter.Current.Show(notification);
                    }
                    else if (Tipo_User == "admin")
                    {
                        var funcion = new ContM_CrearCuenta();
                        await funcion.ValidarCuenta(Correo.Text, Contra.Text);

                        Application.Current.MainPage = new NavigationPage(new NavAdmin());
                    }
                    else if (Tipo_User == "Empleado")
                    {
                        var funcion = new ContM_CrearCuenta();
                        await funcion.ValidarCuenta(Correo.Text, Contra.Text);

                        Application.Current.MainPage = new NavigationPage(new FlyoutEmple1());
                    }
                    //UserDialogs.Instance.HideLoading();
                }
                catch (Exception e)
                {
                    //await DisplayAlert("Error", "Error: " + e, "OK");
                    await DisplayAlert("Aviso", "El correo electrónico o la contraseña son incorrectos.", "OK");
                    //await Navigation.PushAsync(new Login());
                }
            }
        }

        private async void btnrecuperar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Recuperar());
        }
    }
}