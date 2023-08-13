using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VistaTipoPago : ContentPage
    {
        public VistaTipoPago()
        {
            InitializeComponent();
        }

        private async void Btn_PagarSitio(object sender, EventArgs e)
        {
            await DisplayAlert("Aviso", "Gracias por su reservacion.", "OK");
            Application.Current.MainPage = new NavigationPage(new NavCustomer1());
        }

        private void Btn_PagarTarjeta(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new VistaPagos());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }
    }
}