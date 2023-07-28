using ProyectoFinalMovil2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerInfoReservacion : Popup
    {
        public VerInfoReservacion(ReservacionesClientes reservacion)
        {
            InitializeComponent();
            llenarLabels(reservacion);
        }

        private void btnClose_Clicked(object sender, EventArgs e){
            Dismiss(null);
        }

        private void llenarLabels(ReservacionesClientes reservacion){
            lblUsrName.Text = reservacion.Nombre_Usuario;
            lblEstName.Text = reservacion.Nombre_Estilisita;
            lblReservDate.Text = reservacion.Fecha_Reservacion;
            lblReservHour.Text = reservacion.Hora_Reservacion;
            lblReservPrice.Text = reservacion.Precio;
            lblReservType.Text = reservacion.Tipo_Reservacion;
        }
    }
}