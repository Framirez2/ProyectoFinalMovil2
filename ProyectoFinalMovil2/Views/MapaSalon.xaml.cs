using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapaSalon : ContentPage
    {
        double Latitud = 14.086840;
        double Longitud = -87.187070;
        public MapaSalon()
        {
            
            InitializeComponent();

            ObtenerLatitud_Longitud();
        }

        private async void ObtenerLatitud_Longitud()
        {
            try
            {
                var Estado = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (Estado == PermissionStatus.Granted)
                {

                    var locator = CrossGeolocator.Current;
                    bool isGpsEnabled = locator.IsGeolocationEnabled;

                    if (isGpsEnabled)
                    {
                        var pin = new Pin
                        {
                            Position = new Position(Latitud, Longitud),
                            Label = "Ubicacion: " + "Latitud: " + Latitud + "Longitud: " + Longitud
                        };

                        mapa.Pins.Add(pin);
                        //lugares 100 metros
                        mapa.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(100)));


                    }
                    else
                    {
                        // El GPS no está activo
                        await DisplayAlert("Error", "El GPS esta Inactivo por favor activelo", "OK");
                    }
                }
                else
                {
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Los servicios de ubicación no están habilitados en el dispositivo"))
                {
                    await DisplayAlert("Aviso", "Servicio de localizacion no encendido", "OK");
                }
                else
                {
                    
                    await DisplayAlert("Aviso", ex.Message, "OK");
                }
            }
        }

        private async void btnNavegar_Clicked(object sender, EventArgs e)
        {
            var location = new Location(Latitud, Longitud);
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
            await Xamarin.Essentials.Map.OpenAsync(location, options);
        }
    }
}