using ProyectoFinalMovil2.Views;
using System.Runtime.InteropServices;
using Xamarin.Forms;

namespace ProyectoFinalMovil2
{
    public partial class App : Application
    {
        [System.Obsolete]
        public static MasterDetailPage MasterDet { get; set; }
        public App()
        {
            InitializeComponent();


            MainPage = new NavigationPage(new Login());

            //MainPage = new NavigationPage(new CustomerMenu());

            //MainPage = new NavigationPage(new NavCustomer1());

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
