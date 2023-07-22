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
    public partial class ReservacionesFinalizadas : ContentPage
    {
        public ReservacionesFinalizadas()
        {
            InitializeComponent();
        }

        private void listReservaciones_ItemTapped(object sender, ItemTappedEventArgs e) {
            /*private void listReservaciones_ItemTapped(object sender, ItemTappedEventArgs e)
            {

            }*/
        }
    }
}