using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using ProyectoFinalMovil2.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProyectoFinalMovil2.Components;
using ProyectoFinalMovil2.Servicios;
using ProyectoFinalMovil2.Controllers;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Recuperar : ContentPage
    {
        public Recuperar()
        {
            InitializeComponent();
        }

        private async void btnRecuperar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCorreo.Text))
            {
                if (txtCorreo.Text.Contains("@") && txtCorreo.Text.Contains("."))
                {
                    var funcion = new Cont_RestablecerCuenta();
                    var result = await funcion.RestablecerClvae(txtCorreo.Text);
                    if (result)
                    {
                        await App.Current.MainPage.Navigation.PushPopupAsync(new PopUp("Satisfactorio", "El correo electrónico se a enviado satisfactoriamente.", PopUp.Success), true);
                        DependencyService.Get<INotification>().CreateNotification("NailBars", "El correo se a enviado exitosamente, para poder restablecer la contraseña debe dar click en el enlace enviado.");
                        await Navigation.PopAsync();
                    }
                    else await App.Current.MainPage.Navigation.PushPopupAsync(new PopUp("Advertencia", "Por favor revise si su correo electrónico esta escrito correctamente.", PopUp.Warning), true);
                }
                else await App.Current.MainPage.Navigation.PushPopupAsync(new PopUp("Advertencia", "El correo electrónico no es valido.", PopUp.Warning), true);
            }
            else await App.Current.MainPage.Navigation.PushPopupAsync(new PopUp("Advertencia", "Debe ingresar su correo electrónico.", PopUp.Warning), true);

        }
    }
}