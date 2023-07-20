using ProyectoFinalMovil2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoriesServices : ContentPage
	{
        
        Usuarios Datos_Usuario = new Usuarios();
        string Id_Usuario;
        public CategoriesServices ()
		{
			InitializeComponent ();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            string[] descripcion = new string[3];
            descripcion[0] = "- Limado de Pies.";
            descripcion[1] = "- Corte de Uñas.";
            descripcion[2] = "- Exfoliación para Retirar las Celulas Muertas.";
            await Navigation.PushAsync(new Reservaciones(Id_Usuario, 1, "Pedicure", Datos_Usuario, "150", descripcion));
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {

        }
    }
}