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
    public partial class ReservacionesFinalizadas : ContentPage
    {

        ContM_Reservaciones reservaciones;
        List<ReservacionesClientes> reservacionesFinalizadas;

        public ReservacionesFinalizadas()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            try{
                reservacionesFinalizadas = await fillListView();
                listReservaciones.ItemsSource = reservacionesFinalizadas;
            } catch (Exception ex){
                Console.WriteLine(ex.ToString());
                await DisplayAlert("Alerta","Ha ocurrido un error","Ok");
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
            /*Cargar solo las reservaciones que tengan estado finalizadas
             */
        }

        private async Task<List<ReservacionesClientes>> fillListView()
        {
            reservaciones = new ContM_Reservaciones();
            List<ReservacionesClientes> reservacionesFinalizadas = await reservaciones.ObtenerReservacionesFinalizadas();
            return reservacionesFinalizadas;
        }
    }
}