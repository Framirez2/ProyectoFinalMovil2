using ProyectoFinalMovil2.Views;
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


            //MainPage = new NavigationPage(new LoginPruebas());

            //MainPage = new NavigationPage(new CustomerMenu());

            MainPage = new NavigationPage(new AgregarEmpleado());

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
