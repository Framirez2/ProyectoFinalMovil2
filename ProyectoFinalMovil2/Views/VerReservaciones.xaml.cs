using ProyectoFinalMovil2.Controllers;
using ProyectoFinalMovil2.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerReservaciones : ContentPage
    {
        ContM_Reservaciones m_Reservaciones;
        List<ReservacionesClientes> reservacionesEnEspera;

        public VerReservaciones()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                reservacionesEnEspera = await fillListView();
                listReservaciones.ItemsSource = reservacionesEnEspera;
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                await DisplayAlert("Alerta", "Ha ocurrido un error", "Ok");
            }
        }

        private void listReservaciones_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var reserv = e.Item as ReservacionesClientes;
            Navigation.ShowPopup(new VerInfoReservacion(reserv)
            {
                IsLightDismissEnabled = false
            });
            ((ListView)sender).SelectedItem = null;
            /* ver reservaciones cuyo estado no sea "finalizada" 
             */
        }

        private async Task<List<ReservacionesClientes>> fillListView()
        {
            m_Reservaciones = new ContM_Reservaciones();
            List<ReservacionesClientes> reservacionesEnEspera = await m_Reservaciones.ObtenerReservacionesSinFinalizar();
            return reservacionesEnEspera;
        }
    }
}