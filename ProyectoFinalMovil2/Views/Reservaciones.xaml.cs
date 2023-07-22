using ProyectoFinalMovil2.Controllers;
using ProyectoFinalMovil2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamForms.Controls;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Reservaciones : ContentPage
    {
        private string IdUsuario, Precio, FechaReservacion, NombreEstilista = "-", NombreUsuario, HoraReservacion, TipoReservacion;
        private string[] ImageService = { "manicure", "pedicure", "retoquemanospies", "acrilicas" };
        private string[] Servicio = { "Manicure", "Pedicure", "Retoque Acrílico", "Colocación y pintado de uñas acrílicas" };
        private int ContadorEstilista, ContadorReservaciones;

        private Usuarios Data;
        /* public Reservaciones(string idUsuario, int Service, string Reservacion, Usuarios data, string Precio, string[] Descripcion)
         {
             InitializeComponent();
             MostrarEstilistas();
             this.IdUsuario = idUsuario;
             this.TipoReservacion = Reservacion;
             this.Data = data;
             this.Precio = Precio;
             if (this.Data.Tipo_Usuario == "Cliente")
             {
                 txtPrecio.IsEnabled = false;
             }
             else if (this.Data.Tipo_Usuario == "admin")
             {
                 txtPrecio.IsEnabled = true;
             }
             txtServicio.Text = Servicio[Service];
             txtDesc1.Text = Descripcion[0];
             txtDesc2.Text = Descripcion[1];
             txtDesc3.Text = Descripcion[2];
             txtPrecio.Text = "L. " + Precio;
             ImagenServicio(Reservacion);
         }*/

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

        private void txtFechaReservacion_Focused_1(object sender, FocusEventArgs e)
        {

        }

        private void txtFechaReservacion_DateClicked(object sender, XamForms.Controls.DateTimeEventArgs e)
        {

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

        private void txtListaEstilista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtFechaReservacion_DateClicked(object sender, DateChangedEventArgs e)
        {

        }

        private void txtHorario_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtReservar_Clicked(object sender, EventArgs e)
        {

        }

        private void txtFechaReservacion_Focused(object sender, FocusEventArgs e)
        {

        }

        private async Task MostrarEstilistas()
        {
            var funcion = new ContM_Categorias();
            var data = await funcion.ObtenerEstilistas();
            txtListaEstilista.ItemsSource = data;
        }
    }
}