using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavAdminFlyOut : ContentPage
    {
        public ListView ListView;

        public NavAdminFlyOut()
        {
            InitializeComponent();
            BindingContext = new NavAdminFlyOutVM();
            ListView = MenuItemsListView;
        }

        private class NavAdminFlyOutVM : INotifyPropertyChanged
        {

            public ObservableCollection<NavAdminFlyOutMenuItem> MenuItems { get; set; }

            public NavAdminFlyOutVM()
            {
                MenuItems = new ObservableCollection<NavAdminFlyOutMenuItem>(new[]
                {
                    new NavAdminFlyOutMenuItem{Id = 0, Title="Home", IconSource="home.png", TargetType=typeof(NavAdminDetail)},
                    new NavAdminFlyOutMenuItem{Id = 1, Title="Ver Reservaciones", IconSource="ver.png", TargetType=typeof(VerReservaciones)},
                    new NavAdminFlyOutMenuItem{Id = 2, Title="Ver Reservaciones Finalizadas",IconSource="fin.png", TargetType=typeof(ReservacionesFinalizadas)},
                    new NavAdminFlyOutMenuItem{Id = 3, Title="Perfil", IconSource="girl.png", TargetType=typeof(Profile)},
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }


}