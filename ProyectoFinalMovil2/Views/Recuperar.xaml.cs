
using ProyectoFinalMovil2.Controllers;
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
    public partial class Recuperar : ContentPage
    {
        private readonly ContM_Usuarios _RestablecerCuenta = new ContM_Usuarios();
        public Recuperar()
        {
            InitializeComponent();
        }
        private async void btnAtras_Clicked(object sender, EventArgs e)
        {
            await atras();
        }

        private async Task atras()
        {
            await Navigation.PopAsync();
        }
        private async void btnRecuperar_Clicked(object sender, EventArgs e)
        {
            await RecuperarContra();
        }
        private async Task RecuperarContra()
        {
            string correo = txtCorreo.Text;

            //Validar la conexion a internet
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Atencion", "Verifique si tiene conexion a internet", "Aceptar");
                return;
            }
            else
            {

                try
                {

                    if (string.IsNullOrEmpty(correo))
                    {
                        await DisplayAlert(null, "Porfavor ingrese su correo", "Aceptar");
                        return;
                    }

                    bool seEnvio = await _RestablecerCuenta.RecuperarContrasena(correo);
                    if (seEnvio)
                    {
                        await DisplayAlert(null, "Revise su correo electronico", "Ok");
                        //await Xamarin.Essentials.Email.Op;
                    }
                    else
                    {
                        await DisplayAlert("Error", "Correo no valido o no se pudo enviar el correo", "Aceptar");
                    }
                }
                catch (Firebase.Auth.FirebaseAuthException ex)
                {
                    await DisplayAlert("Error", "Correo no valido o inexistente", "Aceptar");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Aceptar");

                }
            }
        }
    }
}