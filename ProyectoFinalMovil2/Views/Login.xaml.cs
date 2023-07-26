using ProyectoFinalMovil2.Controllers;
using ProyectoFinalMovil2.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup;
using Acr.UserDialogs;

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
        }

        private async void btnlogin_Clicked(object sender, EventArgs e)
        {
            await ValidarDatos();
           /* if (!string.IsNullOrEmpty(Correo.Text))
             {
                 if (!string.IsNullOrEmpty(Contra.Text))
                 {

                     //UserDialogs.Instance.ShowLoading("Validando Usuario...");

                     await ValidarDatos();
                 }
                 else
                 {
                     await App.Current.MainPage.Navigation.PopAsync(new JMDialog("Advertencia", "Debe ingresar su contraseña.", JMDialog.Warning), true);
                 }
             }
             else
             {
                 await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar su correo electrónico.", JMDialog.Warning), true);
             }*/

           
        }
        private async void btnregistrar_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new Registro());

        }

        private async Task ValidarDatos()
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
                }
                else if (Tipo_User == "admin")
                {
                    var funcion = new ContM_CrearCuenta();
                    await funcion.ValidarCuenta(Correo.Text, Contra.Text);

                    //Application.Current.MainPage = new NavigationPage(new ContenedorAdmin());
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
                await DisplayAlert("Error", "Error: " + e, "OK");
                //UserDialogs.Instance.HideLoading();
                //await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Aviso", "El correo electrónico o la contraseña son incorrectos.", JMDialog.Danger), true);
                //await Navigation.PushAsync(new Login());
            }
        }

        private async void btnrecuperar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Recuperar());
        }
    }
}