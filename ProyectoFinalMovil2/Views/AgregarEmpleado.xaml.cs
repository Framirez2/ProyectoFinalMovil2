using Firebase.Auth;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
    public partial class AgregarEmpleado : ContentPage
    {
        private MediaFile file;
        private List<string> types;
        private string idUser, fotoRoute, idUserClient;

        public AgregarEmpleado()
        {
            InitializeComponent();
            fillList();
            cmbBoxType.ItemsSource = types;
        }

        private async void btnAgr_Clicked(object sender, EventArgs e)
        {
            if (file != null)
            {
                if (!string.IsNullOrEmpty(txtNames.Text))
                {
                    if (!string.IsNullOrEmpty(txtEmail.Text))
                    {
                        if (!string.IsNullOrEmpty(txtPassword.Text))
                        {
                            if (!string.IsNullOrEmpty(txtRepeatPass.Text))
                            {
                                if (cmbBoxType.SelectedIndex != -1)
                                {
                                    if (txtPassword.Text == txtRepeatPass.Text)
                                    {
                                        if (txtEmail.Text.Contains("@") && txtEmail.Text.Contains("."))
                                        {
                                            if (txtPassword.Text.Trim().Length >= 6)
                                            {
                                                btnAgr.IsEnabled = false;
                                                await createAcount();
                                                await login();
                                                await getUserId();
                                                await uploadImageToStorage();
                                                await saveUser();
                                                await saveWorker();
                                                await DisplayAlert("Alerta", "Usuario ingresado correctamente", "Ok");
                                                clean();
                                                btnAgr.IsEnabled=true;
                                            }
                                            else
                                            {
                                                await DisplayAlert("Alerta", "La contraseña debe tener seis caracteres como minimo", "Ok");
                                            }
                                        }
                                        else
                                        {
                                            await DisplayAlert("Alerta", "El correo electronico ingresado no es correcto!!!", "Ok");
                                        }
                                    }
                                    else
                                    {
                                        await DisplayAlert("Alerta", "Las contraseñas no coinciden!!!", "Ok");
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Alerta", "Por favor seleccione el tipo de usuario!!!", "Ok");
                                }
                            }
                            else
                            {
                                await DisplayAlert("Alerta", "Tiene que repetir la contraseña!!!", "Ok");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Alerta", "Tiene que ingresar una contraseña!!!", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Alerta", "Tiene que ingresar un correo electronico!!!", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Alerta", "Tiene que ingresar su nombre!!!", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Alerta", "Tiene que tomar una foto o seleccionar una!!!", "Ok");
            }
        }

        private async void btnTake_Clicked(object sender, EventArgs e)
        {
            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "NailBars",
                Name = "foto.jpg",
                SaveToAlbum = true
            });
            if (file == null) { return; }
            else imgFoto.Source = ImageSource.FromStream(() => { return file.GetStream(); });
        }

        private async void btnSelect_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null) { return; }
                else
                {
                    imgFoto.Source = ImageSource.FromStream(() =>
                    {
                        var rutaImg = file.GetStream();

                        return rutaImg;
                    });
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        /*private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }*/

        /*private async void btnSelect_Clicked(object sender, EventArgs e){
            await CrossMedia.Current.Initialize();
            try{
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions(){ 
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null){ return; }
                else{
                    imgFoto.Source = ImageSource.FromStream(() =>{
                        var rutaImg = file.GetStream();

                        return rutaImg;
                    });
                }
            }
            catch (Exception ex){ Console.WriteLine(ex.Message); }
        }

        private async void btnTake_Clicked(object sender, EventArgs e){
            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions{
                Directory = "NailBars",
                Name = "foto.jpg",
                SaveToAlbum = true
            });
            if (file == null) { return; }
            else imgFoto.Source = ImageSource.FromStream(() => { return file.GetStream(); });
        }*/

        private void fillList()
        {
            types = new List<string>();
            types.Add("Empleado");
            types.Add("admin");
        }

        private async Task getUserId(){
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));

                var id = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                var refreshC = await authProvider.RefreshAuthAsync(id);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(refreshC));

                idUser = id.User.LocalId;

                Preferences.Remove("MyFirebaseRefreshToken");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task createAcount()
        {   
            ContM_CrearCuenta createFunc = new ContM_CrearCuenta();
            await createFunc.CrearCuentaUser(txtEmail.Text.Trim(), txtPassword.Text.Trim());
        }

        private async Task login()
        {
            ContM_CrearCuenta createFunc = new ContM_CrearCuenta();
            await createFunc.ValidarCuentaUser(txtEmail.Text.Trim(), txtPassword.Text.Trim()); ;
        }

        private async Task uploadImageToStorage()
        {
            ContM_Usuarios createFunc =  new ContM_Usuarios();
            fotoRoute = await createFunc.SubirImagenStorage(file.GetStream(), idUser);
        }

        private async Task saveUser()
        {
            ContM_Usuarios createFunc = new ContM_Usuarios();
            Usuarios user = getUserData();
            idUserClient = await createFunc.InsertarUsuario(user);
            await UpdateUserClienteId();
        }

        private async Task UpdateUserClienteId()
        {
            ContM_Usuarios createFunc = new ContM_Usuarios();
            Usuarios user = new Usuarios();
            user.Id_User = idUser;
            user.Id_User_Cliente = idUserClient;
            user.Nombres = txtNames.Text.Trim().ToString();
            user.Correo = txtEmail.Text.Trim().ToString();
            user.Contrasena = txtPassword.Text.Trim().ToString();
            user.ImagenPerfil = fotoRoute.ToString();
            user.Tipo_Usuario = cmbBoxType.SelectedItem.ToString();
            await createFunc.EditarFotoPerfil(user);
        }

        private async Task saveWorker()
        {
            ContM_Usuarios createFunc = new ContM_Usuarios();
            Trabajadores worker = new Trabajadores();
            worker = getWorkerData();
            await createFunc.GuardarTrabajador(worker);
        }

        private Usuarios getUserData()
        {
            Usuarios user = new Usuarios();
            user.Id_User = idUser;
            user.Id_User_Cliente = "-";
            user.Nombres = txtNames.Text.Trim().ToString();
            user.Correo = txtEmail.Text.Trim().ToString();  
            user.Contrasena = txtPassword.Text.Trim().ToString();
            user.ImagenPerfil = fotoRoute.ToString();
            user.Tipo_Usuario = cmbBoxType.SelectedItem.ToString();

            return user;
        }

        private Trabajadores getWorkerData()
        {
            Trabajadores worker = new Trabajadores();
            worker.Nombres = txtNames.Text.Trim().ToString();
            worker.Foto_Perfil = fotoRoute.ToString();
            return worker;
        }

        private void clean()
        {
            foreach (View item in frmTexts.Children)
            {
                if (item is Entry)
                {
                    (item as Entry).Text = String.Empty;
                }
            }
            imgFoto.Source = "avatar.png";
        }
    }
}