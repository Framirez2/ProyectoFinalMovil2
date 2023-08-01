using ProyectoFinalMovil2.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Recuperar : ContentPage
	{
		Cont_RestablecerCuenta _RestablecerCuenta=new Cont_RestablecerCuenta();
		public Recuperar ()
		{
			InitializeComponent ();
		}
        private async void btnRecuperar_Clicked(object sender, EventArgs e)
        {
            string correo = txtCorreo.Text;
            try
            {

            }catch (Exception ex)
            {
                await DisplayAlert(null, ex.Message, "Aceptar");
            }

            if (string.IsNullOrEmpty(correo))
            {
                await DisplayAlert(null, "Porfavor ingrese su correo","Aceptar");
                return;
            }

            bool seEnvio = await _RestablecerCuenta.RestablecerClvae(correo);
            if (seEnvio)
            {
                await DisplayAlert(null, "Revise su correo electronico", "Ok");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Correo no valido o no se pudo enviar el correo", "Aceptar");
            }
        }
    }
}