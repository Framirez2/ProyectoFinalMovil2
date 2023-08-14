using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavCustomer1Flyout : ContentPage
    {
        public ListView ListView;

        public NavCustomer1Flyout()
        {
            InitializeComponent();

            BindingContext = new NavCustomer1FlyoutViewModel();
            ListView = MenuItemsListView;
        }

        private class NavCustomer1FlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<NavCustomer1FlyoutMenuItem> MenuItems { get; set; }

            public NavCustomer1FlyoutViewModel()
            {
                MenuItems = new ObservableCollection<NavCustomer1FlyoutMenuItem>(new[]
                {
                    new NavCustomer1FlyoutMenuItem { Id = 0, Title = "HOME",IconSource="home.png", TargetType=typeof(NavCustomer1Detail) },
                    new NavCustomer1FlyoutMenuItem { Id = 1, Title = "CATEGORIAS SERVICIOS", IconSource="makeuppouch.png", TargetType=typeof(CategoriesServices) },
                    new NavCustomer1FlyoutMenuItem { Id = 2, Title = "HISTORIAL", IconSource="history.png",  TargetType=typeof(Historial)},
                    new NavCustomer1FlyoutMenuItem { Id = 3, Title = "DASHBOARD", IconSource="layout.png", TargetType=typeof(Dashboard)},
                    new NavCustomer1FlyoutMenuItem { Id = 4, Title = "MAPA", IconSource="googlemaps.png", TargetType=typeof(MapaSalon) },
                    new NavCustomer1FlyoutMenuItem { Id = 4, Title = "PERFIL", IconSource="girl.png", TargetType=typeof(Profile)},
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