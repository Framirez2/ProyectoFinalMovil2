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
	public partial class VistaPagos : ContentPage
	{
		public VistaPagos ()
		{
			InitializeComponent ();
		}

        private async void btnpagar_Clicked(object sender, EventArgs e)
        {
			String Tarjeta = txttarjeta.Text;
			String CVV = txtcvv.Text;
			String Expiracion = txtexpiracion.Text;

			if(Tarjeta == null || CVV == null || Expiracion == null)
			{
				await DisplayAlert("Aviso", "Hay campos vacios", "OK");
			}
			else
			{
                await DisplayAlert("Aviso", "El pago se hizo correctamente", "OK");
            }
        }
    }
}