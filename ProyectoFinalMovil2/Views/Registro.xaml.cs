using Firebase.Auth;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Plugin.Media;
using ProyectoFinalMovil2.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinalMovil2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProyectoFinalMovil2.Components;
using Xamarin.Essentials;
using ProyectoFinalMovil2.Services;
using ProyectoFinalMovil2.Controllers;
using Plugin.LocalNotification;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registro : ContentPage
    {
        MediaFile file;
        string rutafoto, Idusuario, Iduserclientes;

        public Registro()
        {
            InitializeComponent();
        }

        private async void btnCrearcuenta_Clicked(object sender, EventArgs e)
        {

            if (file != null)
            {
                if (!string.IsNullOrEmpty(txtnombres.Text))
                {
                    if (!string.IsNullOrEmpty(txtemail.Text))
                    {
                        if (!string.IsNullOrEmpty(txtcontra.Text))
                        {

                            if (txtemail.Text.Contains("@") && txtemail.Text.Contains("."))
                            {
                                if (txtcontra.Text.Trim().Length >= 6)
                                {                                  
                                    await createAcount();
                                    await login();
                                    await getUserId();
                                    await uploadImageToStorage();
                                    await saveUser();
                                    await DisplayAlert("Advertencia", "Registro Exitoso", "OK");
                                    clean();

                                    var notification = new NotificationRequest
                                    {
                                        BadgeNumber = 1,
                                        Description = "Registro Exitoso",
                                        Title = "Creacion de Cuenta!",
                                        ReturningData = "Dummy Data",
                                        NotificationId = 1337,

                                    };

                                    await LocalNotificationCenter.Current.Show(notification);
                                }
                                else
                                {
                                    await DisplayAlert("Advertencia", "El correo electronico ya existe.", "OK");
                                }
                            }
                            else
                            {
                                await DisplayAlert("Advertencia", "Dede de tener al menos 7 caracteres.", "OK");
                            }
                        }
                        else
                        {
                             await App.Current.MainPage.Navigation.PushPopupAsync(new PopUp("Advertencia", "El correo electrónico no es valido.", PopUp.Warning), true);
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.Navigation.PushPopupAsync(new PopUp("Advertencia", "Debe ingresar su correo electrónico.", PopUp.Warning), true);
                    }
                }
                else
                {
                     await App.Current.MainPage.Navigation.PushPopupAsync(new PopUp("Advertencia", "Debe ingresar su nombre.", PopUp.Warning), true);
                }
            }
            else
            {
                 await App.Current.MainPage.Navigation.PushPopupAsync(new PopUp("Advertencia", "Debe seleccionar o tomar una foto.", PopUp.Warning), true);
            }

        }





        
            private async void btnagregarimagen_Clicked(object sender, EventArgs e)
        {
            //animation.IsVisible = false;
            btnsele.IsVisible = true;

            await CrossMedia.Current.Initialize();
            try
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null)
                {
                    return;
                }
                else
                {
                    btnsele.Source = ImageSource.FromStream(() =>
                    {
                        var rutaImagen = file.GetStream();

                        return rutaImagen;
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }



        private async Task createAcount()
        {
            ContM_CrearCuenta createFunc = new ContM_CrearCuenta();
            await createFunc.CrearCuentaUser(txtemail.Text.Trim(), txtcontra.Text.Trim());
        }

        private async Task login()
        {
            ContM_CrearCuenta createFunc = new ContM_CrearCuenta();
            await createFunc.ValidarCuentaUser(txtemail.Text.Trim(), txtcontra.Text.Trim()); ;
        }
        private async Task uploadImageToStorage()
        {
            ContM_Usuarios createFunc = new ContM_Usuarios();
            rutafoto = await createFunc.SubirImagenStorage(file.GetStream(), Idusuario);
        }
        private async Task saveUser()
        {
            ContM_Usuarios createFunc = new ContM_Usuarios();
            Usuarios user = getUserData();
            Iduserclientes = await createFunc.InsertarUsuario(user);
            await UpdateUserClienteId();
        }

        private Usuarios getUserData()
        {
            Usuarios user = new Usuarios();
            user.Id_User = Idusuario;
            user.Id_User_Cliente = "-";
            user.Nombres = txtnombres.Text.Trim().ToString();
            user.Correo = txtemail.Text.Trim().ToString();
            user.Contrasena = txtcontra.Text.Trim().ToString();
            user.ImagenPerfil = rutafoto.ToString();
            user.Tipo_Usuario = "Cliente";

            return user;
        }

        private async Task UpdateUserClienteId()
        {
            ContM_Usuarios createFunc = new ContM_Usuarios();
            Usuarios user = new Usuarios();
            user.Id_User = Idusuario;
            user.Id_User_Cliente = Iduserclientes;
            user.Nombres = txtnombres.Text.Trim().ToString();
            user.Correo = txtemail.Text.Trim().ToString();
            user.Contrasena = txtcontra.Text.Trim().ToString();
            user.ImagenPerfil = rutafoto.ToString();
            user.Tipo_Usuario = "Cliente";
            await createFunc.EditarFotoPerfil(user);
        }

       
        private async Task getUserId()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));

                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));

                var RefrescarContenido = await authProvider.RefreshAuthAsync(guardarId);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefrescarContenido));
                Idusuario = guardarId.User.LocalId;
            }
            catch (Exception)
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new PopUp("Aviso", "Su sesión a expirado.", PopUp.Danger), true);
            }
        }

        private void clean()
        {
            txtemail.Text = "";
            txtcontra.Text = "";
            txtnombres.Text = "";
            btnsele.Source = "avatar.png";
        }
    }
}