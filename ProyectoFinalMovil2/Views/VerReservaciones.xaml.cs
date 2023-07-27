using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerReservaciones : ContentPage
    {
        public VerReservaciones()
        {
            InitializeComponent();
        }


        private void listReservaciones_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            /* ver reservaciones cuyo estado no sea "finalizada" 
             */
        }
    }
}