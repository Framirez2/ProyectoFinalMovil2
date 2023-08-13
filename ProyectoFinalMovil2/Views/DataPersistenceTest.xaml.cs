using Firebase.Auth;
using Newtonsoft.Json;
using ProyectoFinalMovil2.Controllers;
using ProyectoFinalMovil2.Models;
using ProyectoFinalMovil2.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataPersistenceTest : ContentPage
    {
        string userId;
        string userType;

        public DataPersistenceTest()
        {
            InitializeComponent();
            ObtenerIdUser();
            cerrarSesion();
        }

        private async Task ObtenerIdUser()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));
                var saveId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));

                var refreshContent = await authProvider.RefreshAuthAsync(saveId);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(refreshContent));
                userId = saveId.User.LocalId;
                reLogin();
            }
            catch (Exception ex){  }
        }

        private void cerrarSesion()
        {
            Preferences.Remove("MyFirebaseRefreshToken");
        }

        public async Task reLogin()
        {
            ContM_Usuarios userFunc = new ContM_Usuarios();
            Usuarios user = new Usuarios();
            user.Id_User = userId;
            var data = await userFunc.GetDataMail(user);
            foreach (var fila in data){
                userType = fila.Tipo_Usuario.ToString();
            }

            if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken",""))){
                if (userType == "Cliente"){
                    Application.Current.MainPage = new NavigationPage(new NavCustomer1());
                }
                else if (userType == "admin"){
                    Application.Current.MainPage = new NavigationPage(new NavAdmin());
                }
                else if (userType == "Empleado"){
                    Application.Current.MainPage = new NavigationPage(new FlyoutEmple1());
                }
            }
        }

        private async void btnSignIn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registro());
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }
    }
}