using ProyectoFinalMovil2.Controllers;
using ProyectoFinalMovil2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamForms.Controls;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Reservaciones : ContentPage
    {
        List<string> Hora = new List<string>();
        List<string> HoraDispon = new List<string>();

        private string IdUsuario, Precio, FechaReservacion, NombreEstilista = "-", NombreUsuario, HoraReservacion, TipoReservacion;
        private string[] ImageService = { "manicure", "pedicure", "retoquemanospies", "acrilicas" };
        private string[] Servicio = { "Manicure", "Pedicure", "Retoque Acrílico", "Colocación y pintado de uñas acrílicas" };
        private int ContadorEstilista, ContadorReservaciones;

        private Usuarios Data;

        public Reservaciones(string idUsuario, int serviceIndex, string reservacion, Usuarios data, string precio, string[] descripcion)
        {

            InitializeComponent();
            new Calendar
            {
                BorderColor = Color.Gray,
                BorderWidth = 3,
                BackgroundColor = Color.Gray,
                StartDay = DayOfWeek.Sunday,
                StartDate = DateTime.Now
            };

            // Mostrar la lista de estilistas disponibles.
            MostrarEstilistas();

            // Configurar la información del usuario y el servicio.
            IdUsuario = idUsuario;
            TipoReservacion = reservacion;
            Data = data;
            Precio = precio;

            // Verificar si el usuario actual es "Cliente" y habilitar/deshabilitar el campo de precio en consecuencia.
            bool isCliente = data.Tipo_Usuario == "Cliente";
            txtPrecio.IsEnabled = !isCliente;

            // Mostrar el nombre del servicio y sus descripciones, si están disponibles.
            txtServicio.Text = Servicio[serviceIndex];
            txtDesc1.Text = descripcion.Length > 0 ? descripcion[0] : string.Empty;
            txtDesc2.Text = descripcion.Length > 1 ? descripcion[1] : string.Empty;
            txtDesc3.Text = descripcion.Length > 2 ? descripcion[2] : string.Empty;

            // Mostrar el precio del servicio.
            txtPrecio.Text = "L. " + precio;

            // Mostrar la imagen correspondiente al tipo de reservación del servicio.
            ImagenServicio(reservacion);
        }


        // Asigna la imagen correspondiente al servicio seleccionado a un elemento de imagen (txtImgServicio).
        private void ImagenServicio(string Servicio)
        {
            // Compara el nombre del servicio con las opciones disponibles y asigna la imagen correspondiente.
            if (Servicio.Equals("Manicure"))
            {
                txtImgServicio.Source = ImageService[0];
            }
            else if (Servicio.Equals("Pedicure"))
            {
                txtImgServicio.Source = ImageService[1];
            }
            else if (Servicio.Equals("Aplicacion Acriquilico"))
            {
                txtImgServicio.Source = ImageService[3];
            }
            else if (Servicio.Equals("Pintado y Decoracion"))
            {
                txtImgServicio.Source = ImageService[2];
            }
        }


        /* protected async override void OnAppearing()
         {
             base.OnAppearing();
             txtFechaReservacion.MinimumDate = DateTime.Now;
         }*/

        //Método HorasDisponibles: Este método se encarga de generar una lista de horas disponibles en incrementos de 30 minutos.
        private Task HorasDisponibles()
        {
            // Limpiar la lista de horas (Hora).
            Hora.Clear();
            // Agregar horas disponibles en incrementos de 30 minutos.
            Hora.Add("13:00:00");
            Hora.Add("13:30:00");
            Hora.Add("14:00:00");
            Hora.Add("14:30:00");
            Hora.Add("15:00:00");
            Hora.Add("15:30:00");
            Hora.Add("16:00:00");
            Hora.Add("16:30:00");
            Hora.Add("17:00:00");
            Hora.Add("17:30:00");
            Hora.Add("18:00:00");
            Hora.Add("18:30:00");

            // Devolver una tarea completada, ya que el método no realiza operaciones asincrónicas reales.
            return Task.CompletedTask;
        }

        //Este método se ejecuta cuando se selecciona un elemento en la lista de estilistas.
        private async void txtListaEstilista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //Este método se ejecuta cuando se selecciona un elemento en la lista de estilistas.
                var itemSelected = (Trabajadores)e.CurrentSelection.FirstOrDefault();
                // Obtener el nombre del estilista seleccionado y almacenarlo en la variable "NombreEstilista".
                NombreEstilista = itemSelected.Nombres;
                await DisplayAlert("Aviso", "Estilista seleccionado", "OK");

                // Verificar si la variable "FechaReservacion" tiene un valor válido (no es nula ni vacía).
                if (FechaReservacion != "" && FechaReservacion != null)
                {
                    await GetDataUser();
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        private async Task GetDataUser()
        {
            ContM_Usuarios user = new ContM_Usuarios();

            // Crear una instancia de la clase Usuarios para obtener datos del usuario actual (Muser).
            Usuarios Muser = new Usuarios();
            Muser.Id_User = IdUsuario;
            var D = await user.GetDataUser(Muser);

            // Recorrer la lista de datos obtenidos y actualizar la propiedad "NombreUsuario"
            // con el valor de "Nombres" del usuario actual.
            foreach (var item in D)
            {
                NombreUsuario = item.Nombres;
            }

            // Crear una instancia de la clase ReservacionesClientes para obtener datos relacionados con las reservaciones(Reserv).
            ReservacionesClientes Reserv = new ReservacionesClientes();
            Reserv.Id_Cliente = IdUsuario;
            Reserv.Nombre_Estilisita = NombreEstilista;

            // Obtener las reservaciones del usuario actual para la fecha especificada (FechaReservacion).
            // Almacenar los datos obtenidos en las variables "D2" y "D3"
            var D2 = await user.ContarReservaciones(Reserv, FechaReservacion);
            var D3 = await user.NumeroReserEstilista(Reserv, FechaReservacion);

            // Contar la cantidad de reservaciones del usuario actual y almacenar el resultado en la variable "ContadorReservaciones".
            foreach (var item in D2)
            {
                ContadorReservaciones = ContadorReservaciones + 1;
            }

            // Contar la cantidad de reservaciones del estilista para la fecha especificada y almacenar el resultado en la variable "ContadorEstilista".
            foreach (var item in D3)
            {
                ContadorEstilista = ContadorEstilista + 1;
            }

            string Hour;
            ContM_Reservaciones Funcion = new ContM_Reservaciones();
            ReservacionesClientes GetHours = new ReservacionesClientes();
            GetHours.Fecha_Reservacion = Convert.ToString(txtFechaReservacion);
            GetHours.Nombre_Estilisita = NombreEstilista;
            var HoraBase = await Funcion.GetListB(GetHours, NombreEstilista);

            foreach (var item in HoraBase)
            {
                Hour = item.Hora_Reservacion;
            }
        }

        // Método txtFechaReservacion_DateClicked:
        // Este método se ejecuta cuando se selecciona una fecha en el calendario de reservaciones (txtFechaReservacion).
        private async void txtFechaReservacion_DateClicked(object sender, DateChangedEventArgs e)
        {
            FechaReservacion = String.Concat(e.NewDate.ToString(), "/", e.NewDate.Month.ToString(), "/", e.NewDate.Year.ToString());

            // Verificar si se ha seleccionado un estilista válido (NombreEstilista no es igual a "-").
            if (!NombreEstilista.Equals("-"))
                if (!NombreEstilista.Equals("-"))
                {
                    await GetDataUser();
                    await HorariosEstilista();
                }
                else
                {
                    await DisplayAlert("Aviso", "Por favor, elige un estilista para ver los horarios disponibles.", "OK");
                }
        }

        // Método HorariosEstilista: Este método se encarga de mostrar los horarios disponibles del estilista en la fecha seleccionada.
        private async Task HorariosEstilista()
        {
            // Asignar la lista de horarios disponibles (HoraDispon) al control de la interfaz (txtHorario).
            txtHorario.ItemsSource = HoraDispon;

            ReservacionesClientes horas = new ReservacionesClientes();
            ContM_Reservaciones validar = new ContM_Reservaciones();
            // Asignar la fecha y el nombre del estilista a las propiedades correspondientes del modelo ReservacionesClientes.
            horas.Fecha_Reservacion = FechaReservacion;
            horas.Nombre_Estilisita = NombreEstilista;

            string diaActual = DateTime.Now.ToString("d/MM/yyyy");
            bool esMismoDia = FechaReservacion.Equals(diaActual);

            // Obtener y mostrar las horas disponibles del estilista en la fecha seleccionada, eliminando las horas ocupadas (reservadas).
            await HorasDisponibles();
            var rest = await validar.ObtenerReservacionesEstilista(horas);
            foreach (var rst in rest)
            {
                Hora.Remove(rst.Hora_Reservacion);
            }
            // Si es el mismo día actual, verificar si la hora actual es menor o igual a la hora final (18:30).
            if (esMismoDia)
            {
                Int32 horaActual = DateTime.Now.Hour;
                Int32 minutoActual = DateTime.Now.Minute;
                Int32 horaFinal = 18;
                Int32 minutoFinal = 30;

                // Comprobar si la hora actual es menor o igual a la hora final (18:30).
                bool esHoraMenorOIgual = horaActual < horaFinal || (horaActual == horaFinal && minutoActual <= minutoFinal);

                // Si la hora actual es mayor a la hora final, mostrar un mensaje de alerta indicando que ya no se pueden hacer reservaciones para hoy.
                if (!esHoraMenorOIgual)
                {
                    await DisplayAlert("Aviso", "Ya no se pueden hacer reservaciones para hoy", "OK");
                }
            }

            txtHorario.ItemsSource = Hora;
        }



        private void txtHorario_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtener el horario seleccionado en la lista desplegable y almacenarlo en la variable "HoraReservacion".
            HoraReservacion = txtHorario.SelectedItem.ToString();
        }

        private async void txtReservar_Clicked(object sender, EventArgs e)
        {
            // Verificar si el horario de reservación (HoraReservacion) está vacío o nulo.
            if (string.IsNullOrEmpty(HoraReservacion))
            {
                await DisplayAlert("Aviso", "Falta Hora Reservacion", "OK");
                // Si el horario de reservación está vacío o nulo, redirigir al usuario a la vista de pagos (VistaPagos) y salir del método.
                //Application.Current.MainPage = new NavigationPage(new VistaPagos());
                return;
            }

            ReservacionesClientes reservacion = new ReservacionesClientes();
            ContM_Reservaciones contReser = new ContM_Reservaciones();

            // Verificar si la cantidad de reservaciones y la cantidad de estilistas disponibles cumplen con los límites permitidos (60 y 12 respectivamente).
            if (ContadorReservaciones <= 60 && ContadorEstilista <= 12)
            {
                await DisplayAlert("Aviso", "Creando la Reservacion", "OK");
                // Llenar los datos de la reservación en el objeto "reservacion".
                reservacion.Id_Cliente = IdUsuario;
                reservacion.Nombre_Estilisita = NombreEstilista;
                reservacion.Nombre_Usuario = NombreUsuario;
                reservacion.Tipo_Reservacion = TipoReservacion;
                reservacion.Fecha_Reservacion = FechaReservacion;
                reservacion.Hora_Reservacion = HoraReservacion;
                reservacion.Precio = Precio;
                reservacion.Estado = "Pendiente";
                reservacion.Calificacion = 0;

                // Insertar la reservación en la base de datos a través del controlador ContM_Reservaciones.
                await contReser.InsertarReservacion(reservacion);
                Application.Current.MainPage = new NavigationPage(new VistaTipoPago());
            }
            else if (ContadorReservaciones > 60)
            {
                await DisplayAlert("Aviso", "Seleccione una estilista", "OK");
            }
            else
            {
                await DisplayAlert("Aviso", "Estilista no disponible", "OK");
            }
        }

        private async void txtFechaReservacion_Focused(object sender, FocusEventArgs e)
        {
            if (FechaReservacion == null)
            {
                if (NombreEstilista != "-")
                {
                    FechaReservacion = DateTime.Now.ToString("d/M/yyyy");
                    await HorariosEstilista();
                    await GetDataUser();
                }
            }
        }

        private async Task MostrarEstilistas()
        {
            var funcion = new ContM_Categorias();
            var data = await funcion.ObtenerEstilistas();
            txtListaEstilista.ItemsSource = data;
        }
    }
}