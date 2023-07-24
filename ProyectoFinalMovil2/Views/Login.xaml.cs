using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }


        private async void btnlogin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile());
        }
        private async void btnregistrar_Clicked(object sender, EventArgs e)
        {

        }

    }
}