using Firebase.Auth;
using Newtonsoft.Json;
using ProyectoFinalMovil2.Controllers;
using ProyectoFinalMovil2.Models;
using ProyectoFinalMovil2.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriesServices : ContentPage
    {

        Usuarios Datos_Usuario = new Usuarios();
        string Id_Usuario;
        private FirebaseAuthProvider Autenticacion;
        public CategoriesServices()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            // Crear una lista de strings llamada "descripcion" que contiene elementos con servicios de pedicure.
            List<string> descripcion = new List<string>
            {
                "  Corte y dar forma a las uñas.",
                "  Tratamiento a cutriculas.",
                "  Aplicacion de esmalte."
            };

            // Convertir la lista "descripcion" en un array de strings.
            string[] descripcionArray = descripcion.ToArray();

            // Crear una nueva instancia de la clase "Reservaciones" y pasarle algunos datos como argumentos.
            // El último argumento es un array de strings con los servicios de pedicure.
            await Navigation.PushAsync(new Reservaciones(Id_Usuario, 0, "Manicure", Datos_Usuario, "130", descripcionArray));

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            // Crear una lista de strings llamada "descripcion" que contiene elementos con servicios de pedicure.
            List<string> descripcion = new List<string>
            {
                "  Limado de Pies.",
                "  Corte de uñas.",
                "  Exfoliación para Retirar las Células Muertas."
            };

            // Convertir la lista "descripcion" en un array de strings.
            string[] descripcionArray = descripcion.ToArray();

            // Crear una nueva instancia de la clase "Reservaciones" y pasarle algunos datos como argumentos.
            // El último argumento es un array de strings con los servicios de pedicure.
            await Navigation.PushAsync(new Reservaciones(Id_Usuario, 1, "Pedicure", Datos_Usuario, "150", descripcionArray));
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            // Crear una lista de strings llamada "descripcion" que contiene elementos con servicios de pedicure.
            List<string> descripcion = new List<string>
            {
                "  Corte de uñas.",
                "  Pintado.",
                "  Colocación de uñas."
            };

            // Convertir la lista "descripcion" en un array de strings.
            string[] descripcionArray = descripcion.ToArray();

            // Crear una nueva instancia de la clase "Reservaciones" y pasarle algunos datos como argumentos.
            // El último argumento es un array de strings con los servicios de pedicure.
            await Navigation.PushAsync(new Reservaciones(Id_Usuario, 2, "Pintado y Decoracion", Datos_Usuario, "300", descripcionArray));
        }

        private async void Button_Clicked_3(object sender, EventArgs e)
        {
            // Crear una lista de strings llamada "descripcion" que contiene elementos con servicios de pedicure.
            List<string> descripcion = new List<string>
            {
                "  Diseño sencillo.",
                "  Elegante en Mate.",
                "  Limpieza de uñas."
            };

            // Convertir la lista "descripcion" en un array de strings.
            string[] descripcionArray = descripcion.ToArray();

            // Crear una nueva instancia de la clase "Reservaciones" y pasarle algunos datos como argumentos.
            // El último argumento es un array de strings con los servicios de pedicure.
            await Navigation.PushAsync(new Reservaciones(Id_Usuario, 3, "Aplicacion Acriquilico", Datos_Usuario, "300", descripcionArray));
        }

        // Obtiene el ID de usuario de Firebase y realiza la validación del tipo de usuario.
        private async Task GetUserId()
        {
            try
            {
                // Crear una instancia de FirebaseAuthProvider con la clave de API de Firebase.
                Autenticacion = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));

                // Comprobar si existe la clave "MyFirebaseRefreshToken" en las preferencias.
                if (Preferences.ContainsKey("MyFirebaseRefreshToken"))
                {
                    // Obtener el token de actualización desde las preferencias.
                    var refreshToken = Preferences.Get("MyFirebaseRefreshToken", string.Empty);

                    // Deserializar el token de actualización y obtener el ID de usuario de Firebase.
                    var SaveIdUser = JsonConvert.DeserializeObject<FirebaseAuth>(refreshToken);
                    Id_Usuario = SaveIdUser.User?.LocalId;
                }

                // Llamar al método para validar el tipo de usuario y realizar acciones adicionales.
                await ValidarTipoUsuario();
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, mostrar un aviso de que la sesión ha finalizado.
                await Application.Current.MainPage.DisplayAlert("Aviso", "La sesión ha finalizado", "OK");
            }
        }

        private async Task ValidarTipoUsuario()
        {
            // Crear una instancia del controlador ContM_Usuarios para manejar operaciones con usuarios.
            ContM_Usuarios contM_Usuarios = new ContM_Usuarios();

            // Crear una instancia de Usuarios para almacenar los datos del usuario actual.
            Usuarios usuarios = new Usuarios();
            usuarios.Id_User = Id_Usuario;

            // Obtener información del usuario actual desde la base de datos.
            var Datos = await contM_Usuarios.GetAdmin(usuarios);

            // Recorrer los datos obtenidos (puede haber varios resultados, pero aquí solo se toma el último).
            foreach (var a in Datos)
            {
                // Crear una nueva instancia de Usuarios para almacenar la información del usuario.
                var Datos_Usuario = new Usuarios();

                // Actualizar la información del usuario con los datos obtenidos.
                Datos_Usuario.Tipo_Usuario = a.Tipo_Usuario;
                Datos_Usuario.Id_User_Cliente = a.Id_User_Cliente;
            }
        }

        protected override async void OnAppearing()
        {
            await GetUserId();
        }

    }
}