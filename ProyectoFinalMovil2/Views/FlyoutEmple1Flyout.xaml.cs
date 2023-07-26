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
    public partial class FlyoutEmple1Flyout : ContentPage
    {
        public ListView ListView;

        public FlyoutEmple1Flyout()
        {
            InitializeComponent();

            BindingContext = new FlyoutEmple1FlyoutViewModel();
            ListView = MenuItemsListView;
        }

        private class FlyoutEmple1FlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<FlyoutEmple1FlyoutMenuItem> MenuItems { get; set; }

            public FlyoutEmple1FlyoutViewModel()
            {
                MenuItems = new ObservableCollection<FlyoutEmple1FlyoutMenuItem>(new[]
                {
                    new FlyoutEmple1FlyoutMenuItem { Id = 0, Title = "HOME", IconSource="home.png", TargetType=typeof(FlyoutEmple1Detail)},
                    new FlyoutEmple1FlyoutMenuItem { Id = 1, Title = "HISTORIAL", IconSource="history.png",  TargetType=typeof(Historial) },
                    new FlyoutEmple1FlyoutMenuItem { Id = 2, Title = "PERFIL", IconSource="girl.png", TargetType=typeof(Profile)}, 
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