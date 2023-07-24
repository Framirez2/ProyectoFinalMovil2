using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavCustomer : ContentPage
    {
        public NavCustomer()
        {
            InitializeComponent();
        }

        private async void Button_Inicio(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CategoriesServices());
        }

        private async void Button_Historial(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Historial());
        }
    }
}