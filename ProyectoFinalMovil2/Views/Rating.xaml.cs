using Firebase.Auth;
using Newtonsoft.Json;
using ProyectoFinalMovil2.Controllers;
using ProyectoFinalMovil2.Models;
using ProyectoFinalMovil2.Services;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Rating : PopupPage
    {
        public static string Id_User;
        public static string Id_Reservacion;
        public static string Resena = "";
        public static string Calificacion;
        public static string Id_Calificacion;
        ReservacionesClientes Data_Reserv = new ReservacionesClientes();
        public Rating(ReservacionesClientes itemSelect)
        {
            InitializeComponent();
            GetIdUser();
            Data_Reserv = itemSelect;
            DatosCalificar();

        }

        // Este método se encarga de obtener el ID del usuario actualmente autenticado.
        private async Task GetIdUser()
        {
            try
            {
                // Creamos una instancia de la clase FirebaseAuthProvider y le proporcionamos la clave API de Firebase.
                var AuthProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));
                // Recuperamos el ID del usuario almacenado previamente en las preferencias de la aplicación.
                // Este ID es obtenido del token de autenticación de Firebase que se guardó durante el inicio de sesión.
                // Lo deserializamos para obtener el objeto FirebaseAuth, que contiene la información del usuario.
                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                // Extraemos el ID del usuario localmente autenticado y lo almacenamos en la variable Id_User.
                Id_User = guardarId.User.LocalId;
            }
            catch (Exception)
            {
                //await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Aviso", "Su sesión a expirado.", JMDialog.Danger), true);
                await DisplayAlert("Aviso", "La sesion ha terminado", "OK");
            }
        }

        private async Task DatosCalificar()
        {
            // Creamos una instancia de ContM_Resenas para obtener las reseñas
            var funcion = new ContM_Resenas();

            // Creamos un objeto de tipo Resenas con el ID de la reservación actual
            var parametros = new Resenas();
            parametros.Id_Reservacion = Data_Reserv.Id_Reservaciones;
            // Obtenemos la lista de reseñas usando el método GetDatos del ContM_Resenas
            var dt = await funcion.GetDatos(parametros);

            // Verificamos si hay al menos una reseña en la lista
            if (dt.Count > 0)
            {
                // Si hay reseñas, obtenemos la primera (suponemos que solo hay una reseña por reservación)
                var data = dt[0];
                // Extraemos los datos de la reseña: texto y calificación
                Resena = data.Resena;
                Calificacion = data.Calificacion;
                Id_Calificacion = data.Id_Resena;

                // Verificamos si la reseña no está vacía (no es null o una cadena vacía)
                if (!string.IsNullOrEmpty(Resena))
                {
                    txtreseña.Text = Resena;
                    EstCalificacion.SelectedStarValue = Convert.ToInt32(Calificacion);
                    btnguardar.Text = "Actualizar";
                }
            }
        }

        private async Task InsertRating()
        {
            var calificacion = EstCalificacion.SelectedStarValue.ToString();
            var resena = txtreseña.Text;

            // Validamos que se haya seleccionado una calificación y que la reseña no esté vacía
            if (string.IsNullOrEmpty(calificacion) || string.IsNullOrEmpty(resena))
            {
                await DisplayAlert("Error", "Por favor, selecciona una calificación y escribe tu reseña.", "OK");
                return;
            }

            // Creamos una instancia de ContM_Resenas y Resenas para insertar la nueva reseña
            var funcion = new ContM_Resenas();
            var parametros = new Resenas
            {
                Calificacion = calificacion,
                Resena = resena,
                Id_Usuario = Id_User,
                Id_Reservacion = Data_Reserv.Id_Reservaciones
            };

            // Insertamos la nueva reseña en la base de datos
            await funcion.InsertResenas(parametros);

            // Actualizamos la calificación en la reservación correspondiente
            var consulta = new ContM_Reservaciones();
            await consulta.EditResena(new Resenas { Calificacion = calificacion, Id_Reservacion = Data_Reserv.Id_Reservaciones });

            // Cerramos la página de calificación
            await PopupNavigation.Instance.PopAsync();
        }

        private async Task EditRating()
        {
            var calificacion = EstCalificacion.SelectedStarValue.ToString();
            var resena = txtreseña.Text;

            // Validamos que se haya seleccionado una calificación y que la reseña no esté vacía
            if (string.IsNullOrEmpty(calificacion) || string.IsNullOrEmpty(resena))
            {
                await DisplayAlert("Error", "Por favor, selecciona una calificación y escribe tu reseña.", "OK");
                return;
            }

            // Creamos una instancia de ContM_Resenas y Resenas para editar la reseña existente
            var funcion = new ContM_Resenas();
            var parametros = new Resenas
            {
                Calificacion = calificacion,
                Resena = resena,
                Id_Resena = Id_Calificacion
            };

            // Editamos la reseña en la base de datos
            await funcion.EditResena(parametros);

            // Actualizamos la calificación en la reservación correspondiente
            var consulta = new ContM_Reservaciones();
            await consulta.EditResena(new Resenas { Calificacion = calificacion, Id_Reservacion = Data_Reserv.Id_Reservaciones });

            // Cerramos la página de calificación
            await PopupNavigation.Instance.PopAsync();
        }


        private async void btnguardar_Clicked(object sender, EventArgs e)
        {
            // Verificar si el campo de la reseña está vacío
            if (string.IsNullOrEmpty(txtreseña.Text))
            {
                await DisplayAlert("Aviso", "Debe ingresar una reseña", "OK");
            }
            else
            {
                // Si la variable 'Resena' no está vacía, significa que ya existe una reseña previa
                if (Resena != "")
                {
                    // Llamar a la función para editar la reseña existente
                    await EditRating();
                }
                else
                {
                    // Si no hay reseña previa, llamar a la función para agregar una nueva reseña
                    await InsertRating();
                    //await DatosCalificar();
                    await DisplayAlert("Aviso", "Gracias por su calificación", "OK");

                }
            }
        }

    }
}