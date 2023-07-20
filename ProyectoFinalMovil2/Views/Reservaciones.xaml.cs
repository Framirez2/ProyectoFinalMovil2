using ProyectoFinalMovil2.Controllers;
using ProyectoFinalMovil2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Reservaciones : ContentPage
    {
        private string IdUsuario, Precio, FechaReservacion, NombreEstilista = "-", NombreUsuario, HoraReservacion, TipoReservacion;
        private string[] ImageService = { "manicure", "pedicure", "retoquemanos", "uacrilicas" };
        private string[] Servicio = { "Manicure", "Pedicure", "Retoque Acrílico", "Colocación y pintado de uñas acrílicas" };
        private int ContadorEstilista, ContadorReservaciones;

        private Usuarios Data;
        public Reservaciones(string idUsuario, int Service, string Reservacion, Usuarios data, string Precio, string[] Descripcion)
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
        }

        private void ImagenServicio(string Servicio)
        {
            if (Servicio.Equals("Manicure"))
            {
                txtImgServicio.Source = ImageService[0];
            }
            else if (Servicio.Equals("Pedicure"))
            {
                txtImgServicio.Source = ImageService[1];
            }
            else if (Servicio.Equals("AplicaciondeAcrilico"))
            {
                txtImgServicio.Source = ImageService[3];
            }
            else if (Servicio.Equals("PintadoYDecoracion"))
            {
                txtImgServicio.Source = ImageService[2];
            }
        }

        private void txtListaEstilista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtFechaReservacion_DateSelected(object sender, DateChangedEventArgs e)
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